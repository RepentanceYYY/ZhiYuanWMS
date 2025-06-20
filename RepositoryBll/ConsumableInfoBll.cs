using Entity;
using Entity.DTOModels;
using Entity.Enums;
using IRepositoryBll;
using IRepositoryDal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RepositoryBll
{
    public class ConsumableInfoBll: IConsumableInfoBll
    {
        IConsumableInfoDal _consumableInfoDal;
        ICategoryDal _category;
        IConsumableRecordDal _consumableRecordDal;
        IUserInfoDal _userInfoDal;
        RepositoryDBContext _repositoryDBContext;
        public ConsumableInfoBll(IUserInfoDal userInfoDal,IConsumableInfoDal consumableInfoDal, ICategoryDal category, IConsumableRecordDal consumableRecordDal, RepositoryDBContext repositoryDBContext)
        {
            _consumableInfoDal = consumableInfoDal;
            _category = category;
            _consumableRecordDal = consumableRecordDal;
            _repositoryDBContext = repositoryDBContext;
            _userInfoDal = userInfoDal;
        }

        public bool CreateConsumableInfo(string consumableName, string specification, int num, string unit, decimal money, int warningNum, string categoryId, string description, out string msg)
        {
            msg = "";
            bool exist = _consumableInfoDal.GetEntities().Any(x => x.ConsumableName.Contains(consumableName) && !x.IsDelete);
            if (exist)
            {
                msg = "已存在此耗材";
                return false;
            }
            ConsumableInfo consumableInfo = new ConsumableInfo
            {
                Id = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                ConsumableName = consumableName,
                Specification = specification,
                Num = num,
                Unit = unit,
                Money = money,
                WarningNum = warningNum,
                CategoryId = categoryId,
                Description = description
            };
            bool result= _consumableInfoDal.CreateEntity(consumableInfo);
            if (result)
            {
                msg = "添加成功";
                return true;
            }
            else
            {
                msg = "添加失败";
                return false;
            }

        }

        public bool DeleteConsumableInfo(string id)
        {
            return _consumableInfoDal.DeleteEntity(id);
        }

        public bool DeleteConsumableInfos(List<string> ids)
        {
            return _consumableInfoDal.DeleteEntityByIds(ids);
        }

        public Stream DownLoad()
        {

            //获取项目当前运行的目录
            string directoryPaht=Directory.GetCurrentDirectory();
            //下载文件夹的路径
            string downloadPah= Path.Combine(directoryPaht,"DownlocadFile");            
            //如果没有文件夹就创建
            if (!Directory.Exists(downloadPah))
            {
                //创建文件夹
                Directory.CreateDirectory(downloadPah);
            }
            string filePath=Path.Combine(downloadPah, "Emo.xlsx");
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);

            XSSFWorkbook wk = new XSSFWorkbook();
            //创建表
            ISheet sheet = wk.CreateSheet("Sheet0");            
            #region 设置头部标题
            IRow headrow = sheet.CreateRow(0);
            headrow.CreateCell(0).SetCellValue("耗材名称");
            headrow.CreateCell(1).SetCellValue("规格");
            headrow.CreateCell(2).SetCellValue("类型");
            headrow.CreateCell(3).SetCellValue("数量");
            headrow.CreateCell(4).SetCellValue("购买时间");
            headrow.CreateCell(5).SetCellValue("采购人");

            
            #endregion

            var list = (from c1 in _consumableRecordDal.GetEntities()
                        join c2 in _consumableInfoDal.GetEntities()
                        on c1.ConsumableId equals c2.Id
                        into CRCTemp
                        from cc in CRCTemp.DefaultIfEmpty()

                        join u in _userInfoDal.GetEntities()
                        on c1.Creator equals u.Id
                        into CRUTemp
                        from cu in CRUTemp.DefaultIfEmpty()
                        select (new
                        {
                            cc.ConsumableName,
                            cc.Specification,
                            c1.Type,
                            c1.Num,
                            c1.CreateTime,
                            cu.UserName
                        })).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var datas = list[i];
                IRow row = sheet.CreateRow(i+1);
                row.CreateCell(0).SetCellValue(datas.ConsumableName);
                row.CreateCell(1).SetCellValue(datas.Specification);
                row.CreateCell(2).SetCellValue(datas.Type.ToString());
                row.CreateCell(3).SetCellValue(datas.Num);
                row.CreateCell(4).SetCellValue(datas.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(5).SetCellValue(datas.UserName);

            }
            wk.Write(fileStream);
            //读取刚刚写好的excel文件
            FileStream fileStream1 = new FileStream(filePath,FileMode.Open,FileAccess.Read);
            return fileStream1;

        }

        public ConsumableInfo GetConsumableInfoById(string id)
        {
            return _consumableInfoDal.GetEntityById(id);
        }
        /// <summary>
        /// 获取耗材下拉框
        /// </summary>
        /// <returns></returns>
        public List<OptionsModel> GetConsumableInfoOptions()
        {
            List<OptionsModel> list = _consumableInfoDal.GetEntities().Where(m => !m.IsDelete).Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.ConsumableName
            }).ToList();
            return list;
        }

        /// <summary>
        /// 分页获取耗材信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="consumableName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<GetConsumableInfoDTO> GetConsumableInfosByPage(int limit, int page, string consumableName, out int count)
        {
            var datas = (from co in _consumableInfoDal.GetEntities().Where(c => !c.IsDelete)
                         join ca in _category.GetEntities()
                         on co.CategoryId equals ca.Id
                         into tempCC
                         from cc in tempCC.DefaultIfEmpty()
                         select new
                         {
                             co.Id,
                             co.Description,
                             co.CategoryId,
                             cc.CategoryName,
                             co.ConsumableName,
                             co.Specification,
                             co.Num,
                             co.Unit,
                             co.Money,
                             co.WarningNum,
                             co.CreateTime                             
                         });
            if (!string.IsNullOrEmpty(consumableName))
            {
                datas = datas.Where(c => c.ConsumableName.Contains(consumableName));
            }
            //赋值个数
            count = datas.Count();

            var tmp = datas.OrderByDescending(c => c.CreateTime).Skip((page - 1) * limit).Take(limit).Select(co => new
            {
                co.Id,
                co.Description,
                co.CategoryId,
                co.CategoryName,
                co.ConsumableName,
                co.Specification,
                co.Num,
                co.Unit,
                co.Money,
                co.WarningNum,
                co.CreateTime                
            }).ToList();

            //数据格式转换
            var res = tmp.Select(c =>
            {
                string createTime = c.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                return new GetConsumableInfoDTO
                {
                    Id = c.Id,
                    Description = c.Description,
                    CategoryId = c.CategoryId,
                    ConsumableName = c.ConsumableName,
                    Specification = c.Specification,
                    Num = c.Num + "",
                    Unit = c.Unit,
                    Money = c.Money + "",
                    WarningNum = c.WarningNum + "",
                    CreateTime = createTime,
                    CategoryName = c.CategoryName
                };
            });
            return res.ToList();
        }

        public bool UpdateConsumableInfo(string id, string consumableName, string specification, int num, string unit, decimal money, int warningNum, string categoryId, string description)
        {
            ConsumableInfo consumableInfo = _consumableInfoDal.GetEntityById(id);
            if (consumableInfo == null)
            {
                return false;
            }

            consumableInfo.ConsumableName = consumableName;
            consumableInfo.Specification = specification;
            consumableInfo.Num = num;
            consumableInfo.Unit = unit;
            consumableInfo.Money = money;
            consumableInfo.WarningNum = warningNum;
            consumableInfo.CategoryId = categoryId;
            consumableInfo.Description = description;

            return _consumableInfoDal.UpdateEntity(consumableInfo);
        }

        public bool Upload(Stream stream,string userId,out string msg)
        {
            msg = "";
            XSSFWorkbook wk = new XSSFWorkbook(stream);
            ISheet sheet = wk.GetSheetAt(0);

            #region 获取第一行内的单元格(Title)
            //获取第一行
            IRow row = sheet.GetRow(0);

            string value0 = row.GetCell(0).ToString();
            string value1 = row.GetCell(1).ToString();
            string value2 = row.GetCell(2).ToString();
            string value3 = row.GetCell(3).ToString();
            #endregion

            
            if (value0== "物品名称" && value1== "数量" && value2 == "实际购买数量" && value3 == "规格")
            {
                //打开事务
                using (var transaction = _repositoryDBContext.Database.BeginTransaction())
                {
                    //计数
                    int count = 0;
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        try
                        {
                            IRow row1 = sheet.GetRow(i + 1);
                            string name = row1.GetCell(0).ToString();//获取名称
                            string num = row1.GetCell(1).ToString();//获取数量
                            string realNum = row1.GetCell(2).ToString();//获取实际购买数量
                            string specification = row1.GetCell(3).ToString();//获取规格

                            //通过耗材获取id
                            ConsumableInfo consumableInfo = _consumableInfoDal.GetEntities().FirstOrDefault(x => x.ConsumableName == name);

                            //如果找不到通过名字找不到耗材
                            if (consumableInfo == null)
                            {
                                transaction.Rollback();
                                msg = $"请检查第{i + 2}行耗材名称";                                
                                return false;
                            }
                            int number = 0;
                            //类型转换
                            bool tmp = int.TryParse(realNum, out number);
                            if (!tmp)
                            {
                                transaction.Rollback();
                                msg = $"请检查第{i+2}行数据的购买数量列是否有误";
                                return false;
                            }
                            //累加到耗材表的数量中
                            consumableInfo.Num += number;
                            //添加到耗材记录表中
                            ConsumableRecord consumableRecord = new ConsumableRecord()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreateTime = DateTime.Now,
                                Type = ConsumableRecordTypeEnum.入库,
                                Num = int.Parse(realNum),
                                Creator = userId,
                                ConsumableId = consumableInfo.Id
                            };
                            //添加到耗材记录表
                            _repositoryDBContext.ConsumableRecord.Add(consumableRecord);
                            int index=_repositoryDBContext.SaveChanges();
                            //添加到耗材记录表以及修改耗材表都要成功
                            if (index==2)
                            {
                                count++;
                            }
                            else
                            {
                                transaction.Rollback();
                                msg = $"第{i+2}行耗材";
                            }
                        }
                        catch (Exception)
                        {

                            //回滚事务
                            transaction.Rollback();
                            msg = $"第{i+2}行的耗材添加失败";
                            return false;

                        }
                        
                    }
                    if (count== sheet.LastRowNum)
                    {
                        //提交事务
                        transaction.Commit();
                        msg = "成功";
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        msg = $"失败";
                        return false;
                    }
                }

            }
            else
            {
                msg = "请上传正确的模板";
                return false;
            }
        }
    }
}
