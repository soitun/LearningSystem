using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试所用的试卷
    /// </summary>
    public class ExamTestPaper : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        private static string _pathKey = "ExamTestPaper";

        private static string _virPath = WeiSha.Core.Upload.Get[_pathKey].Virtual;
        private static string _phyPath = WeiSha.Core.Upload.Get[_pathKey].Physics;
    }
}
