using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRepositoryBll
{
    public interface IConsumableInfoBll
    {
        List<GetConsumableInfoDTO> GetConsumableInfosByPage(int limit, int page, string consumableName, out int count);
        bool CreateConsumableInfo(string consumableName, string specification, int num, string unit, decimal money, int warningNum, string categoryId, string description, out string msg);
        bool UpdateConsumableInfo(string id, string consumableName, string specification, int num, string unit, decimal money, int warningNum, string categoryId, string description);
        ConsumableInfo GetConsumableInfoById(string id);
        bool DeleteConsumableInfo(string id);
        bool DeleteConsumableInfos(List<string> ids);
        bool Upload(Stream stream,string userId,out string msg);
        Stream DownLoad();
        List<OptionsModel> GetConsumableInfoOptions();
    }
}
