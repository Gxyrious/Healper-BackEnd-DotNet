using HealperDto.OutDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService
{
    public interface IScaleService
    {
        Dictionary<string, int> FindLabelsWithClient(int clientId);

        List<ScaleRecordInfo> FindScaleRecordInfoByClientId(int clientId, int page, int size);
    }
}
