using HealperDto.OutDto;
using HealperModels;
using System;
using System.Collections.Generic;
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
                .OrderByDescending(o => o.endTime).ToList();
        }
    }
}
