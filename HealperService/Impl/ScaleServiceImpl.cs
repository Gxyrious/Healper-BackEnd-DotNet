using HealperDto.InDto;
using HealperDto.OutDto;
using HealperModels;
using HealperModels.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HealperService.Impl
{
    public class ScaleServiceImpl : IScaleService
    {
        private readonly ModelContext myContext;

        public ScaleServiceImpl(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        public int CountScaleRecordByClientId(int clientId)
        {
            return myContext.ScaleRecords.Count(c => c.ClientId == clientId);
        }

        public void DeleteScaleRecord(int id)
        {
            var record = myContext.ScaleRecords.Single(s => s.Id == id);
            myContext.ScaleRecords.Remove(record);
            myContext.SaveChanges();
        }

        public List<ScaleInfo> FindBasicScales(int page, int size)
        {
            return myContext.PsychologyScales.Select(s => new ScaleInfo
            {
                id = s.Id,
                quesNum = s.QuesNum,
                name = s.Name,
                image = s.Image,
                summary = s.Summary
            }).OrderBy(o => o.id)
            .Skip(size * (page - 1)).Take(size)
            .ToList();
        }

        public Dictionary<string, int> FindLabelsWithClient(int clientId)
        {
            List<string> records = myContext.ScaleRecords
                .Where(w => w.ClientId == clientId)
                .Select(s => s.Record)
                .ToList();
            Dictionary<string, int> factorValues = new();
            foreach (object? obj in (JsonArray)JsonNode.Parse(records[0])!)
            {
                JsonObject map = (JsonObject)obj!;
                string factor = map["factor"]!.ToString();
                factorValues[factor] = 0;
            }
            foreach (string json in records)
            {
                foreach (object? obj in (JsonArray)JsonNode.Parse(json)!)
                {
                    JsonObject map = (JsonObject)obj!;
                    string factor = map["factor"]!.ToString();
                    int value = int.Parse(map["value"]!.ToString());
                    factorValues[factor] += value;
                }
            }
            return factorValues;
        }

        public List<ScaleRecordInfo> FindScaleRecordInfoByClientId(int clientId, int page, int size)
        {
            return myContext.ScaleRecords
                .Where(w => w.ClientId == clientId)
                .Join(myContext.PsychologyScales, s => s.ScaleId, p => p.Id, (s, p) => new ScaleRecordInfo
                {
                    scaleRecordId = s.Id,
                    endTime = s.EndTime,
                    isHidden = s.IsHidden,
                    scaleId = s.ScaleId,
                    record = s.Record,
                    scaleName = p.Name
                })
                .OrderByDescending(o => o.endTime)
                .Skip(size * (page - 1)).Take(size).ToList();
        }

        public ScaleRecordInfo FindScaleRecordInfoById(int recordId)
        {
            return myContext.ScaleRecords
                .Where(w => w.Id == recordId)
                .Join(myContext.PsychologyScales, sr => sr.ScaleId, ps => ps.Id, (sr, ps) => new ScaleRecordInfo
                {
                    scaleRecordId = sr.Id,
                    endTime = sr.EndTime,
                    isHidden = sr.IsHidden,
                    scaleId = sr.ScaleId,
                    record = sr.Record,
                    scaleName = ps.Name
                }).FirstOrDefault()!;
        }

        public PsychologyScale FindSingleScale(int scaleId)
        {
            return myContext.PsychologyScales.FirstOrDefault(f => f.Id == scaleId)!;
        }

        public JsonNode GetJsonScaleByClientId(int clientId)
        {
            JsonArray jsonResult = new();
            List<ScaleRecordInfo> records = myContext.ScaleRecords
                .Where(w => w.ClientId == clientId)
                .Join(myContext.PsychologyScales, s => s.ScaleId, p => p.Id, (s, p) => new ScaleRecordInfo
                {
                    scaleRecordId = s.Id,
                    endTime = s.EndTime,
                    isHidden = s.IsHidden,
                    scaleId = s.ScaleId,
                    record = s.Record,
                    scaleName = p.Name
                })
                .OrderByDescending(o => o.endTime)
                .Take(10).ToList();
            if (records.Count == 0)
            {
                throw new Exception("No Scale Record");
            } else
            {
                string json = records.First().record;
                JsonArray factors = (JsonArray)JsonNode.Parse(json)!;
                foreach (var obj in factors)
                {
                    string factor = obj!["factor"]!.ToString();
                    JsonObject factorRecords = new JsonObject();
                    factorRecords.Add("factor", factor);
                    factorRecords.Add("detail", new JsonArray());
                    jsonResult.Add(factorRecords);
                }

                foreach (ScaleRecordInfo record in records)
                {
                    long endTime = record.endTime;
                    factors = (JsonArray)JsonNode.Parse(record.record)!;
                    for (int j = 0; j < factors.Count; j++)
                    {
                        var obj = factors[j];
                        string value = obj!["value"]!.ToString();
                        JsonArray detail = (JsonArray)jsonResult[j]!["detail"]!;
                        JsonObject info = new JsonObject();
                        info.Add("time", endTime);
                        info.Add("value", value);
                        detail.Add(info);
                    }
                }
                return jsonResult;
            }
        }

        public ScaleRecord UpdateScaleRecord(ScaleRecordInDto scaleRecord)
        {
            ScaleRecord newRecord = new ScaleRecord();
            newRecord.Id = myContext.ScaleRecords.Max(m => m.Id) + 1;
            newRecord.ClientId = scaleRecord.clientId;
            newRecord.EndTime = scaleRecord.endTime;
            newRecord.IsHidden = scaleRecord.isHidden;
            newRecord.ScaleId = scaleRecord.scaleId;
            newRecord.Record = scaleRecord.record;
            newRecord.Subjective = scaleRecord.subjective;
            myContext.ScaleRecords.Add(newRecord);
            myContext.SaveChanges();
            return newRecord;
        }
    }
}
