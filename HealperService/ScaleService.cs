using HealperDto.InDto;
using HealperDto.OutDto;
using HealperModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HealperService
{
    public interface IScaleService
    {
        Dictionary<string, int> FindLabelsWithClient(int clientId);

        List<ScaleRecordInfo> FindScaleRecordInfoByClientId(int clientId, int page, int size);

        ScaleRecordInfo FindScaleRecordInfoById(int recordId);

        JsonNode GetJsonScaleByClientId(int clientId);

        int CountScaleRecordByClientId(int clientId);

        ScaleRecord UpdateScaleRecord(ScaleRecordInDto scaleRecord);

        void DeleteScaleRecord(int id);

        List<ScaleInfo> FindBasicScales(int page, int size);

        PsychologyScale FindSingleScale(int scaleId);
    }
}
