using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using JianTongBLL;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.IO;

namespace JianTongFun
{
    public partial class EntityFun
    {
        private static EntityFun _entity = null;
        public static EntityFun CreateInstance
        {
            get
            {
                if (_entity == null)
                {
                    _entity = new EntityFun();
                }
                return _entity;
            }
        }

        public EntityFun()
        {
            Database.SetInitializer<EntityContext>(null);
        }

        public void Init()
        {
            /*using(EntityContext entity = new EntityContext())
            {
                //entity.Database.Delete();
                entity.Database.CreateIfNotExists();
            }*/
        }

        #region 基础方法

        public T GetTByIntId<T>(int id) where T : IntIdClass
        {
            if (id <= 0)
            {
                return null;
            }
            using (EntityContext entity = new EntityContext())
            {
                T dbObject = entity.GetObjectByT<T>() != null ? entity.GetObjectByT<T>().SingleOrDefault(u => u.Id == id && !u.IsDelete) : null;
                return dbObject;
            }
        }

        public T GetTByStringId<T>(string id) where T:StringIdClass
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            using(EntityContext entity = new EntityContext())
            {
                T dbObject = entity.GetObjectByTStringId<T>() != null ? entity.GetObjectByTStringId<T>().SingleOrDefault(u => u.Id == id && !u.IsDelete) : null;
                return dbObject;
            }
        }

        public List<T> GetListTByIntId<T>(string name,int pageSize,int pageIndex, out int  Count) where T:IntIdClass
        {
            Count = 1;
            using(EntityContext entity = new EntityContext())
            {
                var objList = entity.GetObjectByT<T>();
                if(objList!=null && objList.Count() > 0)
                {
                    IQueryable<T> list = objList.Where(w => !w.IsDelete);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        list = list.Where(w => w.Name.Contains(name));
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        public bool DeleteTByIntIds<T>(string[] ids) where T : IntIdClass
        {
            if (ids == null || ids.Count() <= 0)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<T> list = entity.GetObjectByT<T>();
                if (list != null)
                {
                    list = list.Where(t => ids.Contains(t.Id.ToString()));

                    if (list != null)
                    {
                        foreach (T t in list)
                        {
                            t.LastTime = DateTime.Now;
                            t.IsDelete = true;
                        }
                        return entity.SaveChanges() > 0;
                    }
                }
                return false;
                
            }
        }

        public bool DeleteTByStringIds<T>(string[] ids) where T : StringIdClass
        {
            if (ids == null || ids.Count() <= 0)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<T> list = entity.GetObjectByTStringId<T>();
                if (list != null)
                {
                    list = list.Where(t => ids.Contains(t.Id));

                    if (list != null)
                    {
                        foreach (T t in list)
                        {
                            t.LastTime = DateTime.Now;
                            t.IsDelete = true;
                        }
                        return entity.SaveChanges() > 0;
                    }
                }
                return false;

                //foreach (string id in ids)
                //{
                //    int dbId = OperationFunction.CreateInstance.StringConvertToInt(id, 0);
                //    if (dbId > 0)
                //    {
                //        T dbObject = entity.GetObjectsByT<T>() != null ? entity.GetObjectsByT<T>().SingleOrDefault(u => u.Id == dbId && !u.IsDelete) : null;
                //        if (dbObject != null)
                //        {
                //            dbObject.IsDelete = true;
                //        }
                //    }
                //}
                //return entity.SaveChanges() > 0;
            }
        }

        public bool AddOrUpdateTByIntId<T>(int id, FormCollection form) where T : IntIdClass
        {
            if (form == null || form.Count <= 0)
            {
                return false;
            }
            using (EntityContext entity = new EntityContext())
            {
                T t = null;
                if (id > 0)
                {
                    t = entity.GetObjectByT<T>() != null ? entity.GetObjectByT<T>().SingleOrDefault(u => u.Id == id) : null;
                }
                else
                {
                    Type tType = typeof(T);
                    t = Activator.CreateInstance(tType, true) as T;
                    t.CreateTime = DateTime.Now;
                }

                if (t == null)
                {
                    return false;
                }

                PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length <= 0)
                {
                    return false;
                }



                foreach (PropertyInfo item in properties)
                {
                    string value = form["txt" + item.Name];
                    if (value != null)
                    {
                        if (!SetPropertyInfoValue<T>(item, t, value))
                        {
                            return false;
                        }
                    }
                }

                if (id <= 0)
                {
                    entity.AddTByIntId<T>(t);
                }

                t.LastTime = DateTime.Now;
                return entity.SaveChanges() > 0;
            }
        }

        #endregion

        #region 组织信息相关

        /// <summary>
        /// 根据用户名密码获取用户对象（登陆用）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public JianTongUserClass GetUserByPassword(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            using (EntityContext entity = new EntityContext())
            {
                string Encrypt = OperationFunction.CreateInstance.DESEncrypt(password, "JianTong");
                JianTongUserClass dbUser = entity.JianTongUserClass.SingleOrDefault(user => user.Name == userName && user.UserPwd == Encrypt && !user.IsDelete);
                return dbUser;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public List<JianTongUserClass> GetUsers(int companyId, int departmentId, string positionId, string userName, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<JianTongUserClass> list = entity.JianTongUserClass.Where(user => !user.IsDelete);
                if (list != null)
                {

                    if (!string.IsNullOrWhiteSpace(positionId) && positionId != "-1")
                    {
                        list = list.Where(user => user.Position.Contains(positionId));
                    }
                    else
                    {
                        if (companyId > 0)
                        {
                            list = list.Where(user => user.company == companyId);
                        }

                        if (departmentId > 0 && list != null)
                        {
                            list = list.Where(user => user.department == departmentId);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(userName) && list != null)
                    {
                        list = list.Where(user => user.TrueName.Contains(userName));
                    }
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="userName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private List<JianTongUserClass> GetUsers(string[] companyId, string[] departmentId)
        {
            using (EntityContext entity = new EntityContext())
            {
                IQueryable<JianTongUserClass> list = entity.JianTongUserClass.Where(user => !user.IsDelete);
                if (list != null)
                {
                    if (companyId != null && companyId.Count() > 0)
                    {
                        list = list.Where(user => companyId.Contains(user.company.ToString()));
                    }

                    if (departmentId != null && departmentId.Count() > 0 && list != null)
                    {
                        list = list.Where(user => departmentId.Contains(user.department.ToString()));
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        //获取部门列表
        public List<Department> GetDepartment(int companyId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;
            using (EntityContext entity = new EntityContext())
            {
                IQueryable<Department> list = entity.Department.Where(d => !d.IsDelete);
                if (list != null)
                {
                    if (companyId > 0)
                    {
                        list = list.Where(d => d.company == companyId);
                    }
                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        //获取职位列表
        public List<Position> GetPosition(int companyId, int departmentId, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<Position> list = entity.Position.Where(d => !d.IsDelete);
                if (list != null)
                {
                    if (companyId > 0)
                    {
                        list = list.Where(d => d.company == companyId);
                    }

                    if (departmentId > 0)
                    {
                        list = list.Where(d => d.department == departmentId);
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        //获取公司列表
        public List<Company> GetCompany(string companyName, int pageSize, int pageIndex, out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<Company> list = entity.Company.Where(d => !d.IsDelete);
                if (list != null)
                {
                    if (!string.IsNullOrWhiteSpace(companyName))
                    {
                        list = list.Where(d => d.Name.Contains(companyName));
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        //获取建设单位列表
        public List<DevelopUnit> GetDevelopUnit(string developName,int pageSize,int pageIndex,out int Count)
        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<DevelopUnit> list = entity.DevelopUnit.Where(d => !d.IsDelete);
                if (list != null)
                {
                    if (!string.IsNullOrWhiteSpace(developName))
                    {
                        list = list.Where(d => d.Name.Contains(developName));
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }

        //获取施工单位列表
        public List<ConstructionUnit> GetConstructionUnit(string conName,int pageSize,int pageIndex,out int Count)

        {
            Count = 1;

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<ConstructionUnit> list = entity.ConstructionUnit.Where(d => !d.IsDelete);
                if (list != null)
                {
                    if (!string.IsNullOrWhiteSpace(conName))
                    {
                        list = list.Where(d => d.Name.Contains(conName));
                    }

                    if (pageSize > 0 && pageIndex > 0)
                    {
                        if (list != null && list.Count() > 0)
                        {
                            int dex = list.Count() % pageSize;
                            Count = list.Count() / pageSize;
                            if (dex != 0)
                            {
                                Count += 1;
                            }
                            list = list.OrderByDescending(w => w.Id);
                            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                        }
                    }
                    return list != null ? list.ToList() : null;
                }
                return null;
            }
        }
        //修改登录密码
        public bool ChangeUserLoginPwd(int userId, string oldPwd, string newPwd)
        {
            if (userId <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(oldPwd) || string.IsNullOrWhiteSpace(newPwd))
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass user = entity.JianTongUserClass.SingleOrDefault(e => e.Id == userId && !e.IsDelete);
                string Encrypt = OperationFunction.CreateInstance.DESEncrypt(newPwd, "JianTong");
                if (user != null)
                {
                    user.UserPwd = Encrypt;
                    return entity.SaveChanges() > 0;
                }
                return false;
            }
        }

        //重置用户密码
        public bool ResetUserLoginPwd(int userId)
        {
            if (userId < 0)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass user = entity.JianTongUserClass.SingleOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    string newPwd = OperationFunction.CreateInstance.DESEncrypt("123456", "JianTong");
                    user.UserPwd = newPwd;
                    entity.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool AddOrUpdateCompany(int id, FormCollection form, JianTongUserClass user, HttpFileCollectionBase files)
        {
            if (form == null || form.Count <= 0)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                Company company = null;
                if (id > 0)
                {
                    company = entity.Company.SingleOrDefault(c => c.Id == id && !c.IsDelete);
                }
                else
                {
                    company = new Company();
                    company.CreateTime = DateTime.Now;
                }

                if (company == null)
                {
                    return false;
                }

                PropertyInfo[] properties = company.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length <= 0)
                {
                    return false;
                }

                foreach (PropertyInfo item in properties)
                {
                    string value = form["txt" + item.Name];
                    if (value != null)
                    {
                        if (!SetPropertyInfoValue<Company>(item, company, value))
                        {
                            return false;
                        }
                    }
                }

                if (files != null && files.Count > 0)
                {
                    foreach (string file in files)
                    {
                        HttpPostedFileBase file1 = files[file] as HttpPostedFileBase;
                        if (file1 == null || file1.ContentLength == 0)
                            continue;
                        string path = AppDomain.CurrentDomain.BaseDirectory + "Upload\\Company\\" + company.Id + "\\";
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        string fileName = Path.GetFileName(file1.FileName);
                        company.BarCode = fileName;
                        string savedFileName = Path.Combine(path, fileName);
                        file1.SaveAs(savedFileName);
                    }
                }
                if (id <= 0)
                {
                    entity.Company.Add(company);
                }
                company.LastTime = DateTime.Now;
                return entity.SaveChanges() > 0;
            }
        }

        public bool AddOrUpdatePosition(int id, FormCollection form, JianTongUserClass user)
        {
            if (form == null || user == null)
            {
                return false;
            }
            string company = form["txtcompany"];
            string department = form["txtdepartment"];
            string PositionDescription = form["txtPositionDescription"];
            string Condition = form["txtCondition"];
            string Direction = form["txtDirection"];
            string IsTopPosition = form["txtIsTopPosition"];
            string Name = form["txtName"];
            using (EntityContext entity = new EntityContext())
            {
                Position position = null;
                if (id > 0)
                {
                    position = entity.Position.SingleOrDefault(p => p.Id == id && !p.IsDelete);
                }
                else
                {
                    position = new Position();
                }

                if (position == null)
                {
                    return false;
                }

                position.Name = Name;
                position.PositionDescription = PositionDescription;
                position.Condition = Condition;
                position.Direction = Direction;
                int comId = OperationFunction.CreateInstance.StringConvertToInt(company, 0);
                int de = OperationFunction.CreateInstance.StringConvertToInt(department, 0);
                position.department = de;
                position.company = comId;
                bool isTop = !string.IsNullOrWhiteSpace(IsTopPosition) && IsTopPosition == "true";
                if (isTop)
                {
                    IQueryable<Position> pList = entity.Position.Where(p => p.department == de && !p.IsDelete);
                    if (pList != null && pList.Count() > 0)
                    {
                        foreach (Position p in pList)
                        {
                            p.IsTopPosition = false;
                        }
                    }
                    position.IsTopPosition = true;
                }
                if (id <= 0)
                {
                    entity.Position.Add(position);
                }
                entity.SaveChanges();
                return true;
            }
        }

        public bool AddOrUpdateUserInfo(int userId,FormCollection form)
        {
            if (form == null || form.Count <= 0)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                JianTongUserClass userInfo = null;
                if (userId > 0)
                {
                    userInfo = entity.JianTongUserClass.SingleOrDefault(c => c.Id == userId && !c.IsDelete);
                }
                else
                {
                    userInfo = new JianTongUserClass();
                    userInfo.CreateTime = DateTime.Now;
                }

                if (userInfo == null)
                {
                    return false;
                }

                PropertyInfo[] properties = userInfo.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length <= 0)
                {
                    return false;
                }

                foreach (PropertyInfo item in properties)
                {
                    string value = form["txt" + item.Name];
                    if (value != null)
                    {
                        if (!SetPropertyInfoValue<JianTongUserClass>(item, userInfo, value))
                        {
                            return false;
                        }
                    }
                }
                if (userId <= 0)
                {
                    userInfo.UserPwd = "Df/bhVnuS/4=";
                    entity.JianTongUserClass.Add(userInfo);
                }
                userInfo.LastTime = DateTime.Now;
                return entity.SaveChanges() > 0;
            }
        }

        public bool JudgeUserIsExist(string userAccount)
        {
            if (string.IsNullOrWhiteSpace(userAccount))
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                IQueryable<JianTongUserClass> users = null;
                if (!string.IsNullOrWhiteSpace(userAccount))
                {
                    users = entity.JianTongUserClass.Where(u => u.Name == userAccount);
                    if (users.Count() > 0)
                    {
                        return true;
                    }
                }
               
                return false;
            }
        }

        public List<RoleJurisdiction> GetUserRoleJurisdiction(int rjId, bool isLeftColumn)
        {
            using (EntityContext entity = new EntityContext())
            {
                if (isLeftColumn)
                {
                    IQueryable<RoleJurisdiction> role = entity.RoleJurisdiction.Where(j => j.BeforeId == 0 && !j.IsDelete);
                    if (role != null)
                    {
                        role = role.OrderBy(r => r.JType);
                        return role.ToList();
                    }
                }
                else
                {
                    IQueryable<RoleJurisdiction> role = null;
                    if (rjId > 0)
                    {
                        role = entity.RoleJurisdiction.Where(j => j.BeforeId == rjId && !j.IsDelete);
                    }
                    else
                    {
                        role = entity.RoleJurisdiction.Where(j => j.BeforeId != 0 && !j.IsDelete);
                    }
                    if (role != null)
                    {
                        return role.ToList();
                    }
                }
                return null;
            }
        }

        public bool AddOrUpdateUserRole(int roleId,FormCollection form)
        {
            if (form == null || form.Count <= 0)
            {
                return false;
            }

            using (EntityContext entity = new EntityContext())
            {
                UserRole userInfo = null;
                if (roleId > 0)
                {
                    userInfo = entity.UserRole.SingleOrDefault(c => c.Id == roleId && !c.IsDelete);
                }
                else
                {
                    userInfo = new UserRole();
                    userInfo.CreateTime = DateTime.Now;
                }

                if (userInfo == null)
                {
                    return false;
                }

                PropertyInfo[] properties = userInfo.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (properties.Length <= 0)
                {
                    return false;
                }

                foreach (PropertyInfo item in properties)
                {
                    string value = form["txt" + item.Name];
                    if (value != null)
                    {
                        if (!SetPropertyInfoValue<UserRole>(item, userInfo, value))
                        {
                            return false;
                        }
                    }
                }
                if (roleId <= 0)
                {
                    entity.UserRole.Add(userInfo);
                }
                userInfo.LastTime = DateTime.Now;
                return entity.SaveChanges() > 0;
            }
        }

        public bool JudgeHasJurisdiction(UserRole role, string JurisdictionCode)
        {
            if (role == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(JurisdictionCode))
            {
                return false;
            }

            return role.QuanXian.Contains(JurisdictionCode+",");
        }
        #endregion

        private bool SetPropertyInfoValue<T>(PropertyInfo item, T t, string value) where T : IntIdClass
        {
            if (item == null)
            {
                return false;
            }

            try
            {
                if (item.PropertyType.Equals(typeof(string)))
                {
                    item.SetValue(t, value);
                    return true;
                }

                if (item.PropertyType.Equals(typeof(int)))
                {
                    int v = 0;
                    bool result = Int32.TryParse(value, out v);
                    if (result)
                    {
                        item.SetValue(t, v);
                    }
                    item.SetValue(t, v);
                    return true;
                }

                if (item.PropertyType.Equals(typeof(DateTime)))
                {
                    DateTime v = DateTime.MinValue;
                    bool result = DateTime.TryParse(value, out v);
                    if (result)
                    {
                        item.SetValue(t, v);
                    }
                    return true;
                }

                if (item.PropertyType.Equals(typeof(float)))
                {
                    float v = 0;
                    bool result = float.TryParse(value, out v);
                    if (result)
                    {
                        item.SetValue(t, v);
                    }
                    return true;
                }

                if (item.PropertyType.Equals(typeof(decimal)))
                {
                    decimal v = 0;
                    bool result = decimal.TryParse(value, out v);
                    if (result)
                    {
                        item.SetValue(t, v);
                    }
                    return true;
                }

                if (item.PropertyType.Equals(typeof(bool)))
                {
                    bool v = false;
                    bool result = Boolean.TryParse(value, out v);
                    item.SetValue(t, v);
                    return true;
                }
                return false;
            }
            catch (Exception ce)
            {
                return false;
            }
        }
    }
}
