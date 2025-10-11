using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Song.ViewData;

namespace Song.ViewData.Methods
{
    public class ExamQues : ViewMethod, IViewAPI
    {
        
        public ListResult Pager(int orgid, int index, int size)
        {
            int sum = 0;
            List<Questions> list = Business.Do<IExamQues>().Pager(orgid, null, null, null, null, null, size, index, out sum);
           
            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
    }
}
