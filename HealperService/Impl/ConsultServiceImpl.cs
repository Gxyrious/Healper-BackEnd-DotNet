using HealperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService.Impl
{
    public class ConsultServiceImpl : IConsultService
    {
        private readonly ModelContext myContext;

        public ConsultServiceImpl(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        public bool EndConsultation(int orderId, long endTime)
        {
            try
            {
                var order = myContext.ConsultHistories.Single(s => s.Id == orderId);
                order.EndTime = endTime;
                myContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StartConsultation(int orderId, long startTime)
        {
            try
            {
                var order = myContext.ConsultHistories.Single(s => s.Id == orderId);
                order.StartTime = startTime;
                myContext.SaveChanges();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
