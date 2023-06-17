using HealperDto.InDto;
using HealperDto.OutDto;
using HealperModels.Models;
using HealperResponse;
using HealperService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScaleController : ControllerBase
    {
        private readonly IScaleService myScaleService;

        public ScaleController(IScaleService scaleService)
        {
            myScaleService = scaleService;
        }

        [HttpGet("records")]
        public ResponseEntity GetScaleRecord(int clientId, int page, int size)
        {
            try
            {
                List<ScaleRecordInfo> scaleRecordInfos = myScaleService.FindScaleRecordInfoByClientId(clientId, page, size);
                return ResponseEntity.OK().Body(scaleRecordInfos);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("record")]
        public ResponseEntity GetSingleRecordById(int recordId)
        {
            try
            {
                ScaleRecordInfo recordInfo = myScaleService.FindScaleRecordInfoById(recordId);
                return ResponseEntity.OK().Body(recordInfo);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("jsonRecords")]
        public ResponseEntity GetTotalScaleByClientId(int clientId)
        {
            try
            {
                JsonNode json = myScaleService.GetJsonScaleByClientId(clientId);
                return ResponseEntity.OK().Body(json);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("sum")]
        public ResponseEntity GetScaleRecordNum(int clientId)
        {
            try
            {
                int sum = myScaleService.CountScaleRecordByClientId(clientId);
                return ResponseEntity.OK().Body(sum);
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR("Id Not Exist").Body(err);
            }
        }

        [HttpPost("update")]
        public ResponseEntity UpdateScaleRecord(ScaleRecordInDto inDto)
        {
            try
            {
                ScaleRecord record = myScaleService.UpdateScaleRecord(inDto);
                return ResponseEntity.OK().Body(record);
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpDelete("delete")]
        public ResponseEntity DeleteScaleRecord(int id)
        {
            try
            {
                myScaleService.DeleteScaleRecord(id);
                return ResponseEntity.OK();
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("names")]
        public ResponseEntity GetPsychologyScales(int page, int size)
        {
            try
            {
                List<ScaleInfo> names = myScaleService.FindBasicScales(page, size);
                return ResponseEntity.OK().Body(names);
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("single")]
        public ResponseEntity GetSingleScaleById(int scaleId)
        {
            try
            {
                PsychologyScale scale = myScaleService.FindSingleScale(scaleId);
                return ResponseEntity.OK().Body(scale);
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }
    }
}
