using System;
using System.Data;

namespace SqlFu.Providers
{
   public class OracleProvider : AbstractProvider
    {
        public const string ProviderName = "Oracle.DataAccess.Client";
        public OracleProvider(string provider = ProviderName)
            : base(provider ?? ProviderName)
        {

        }

        public override void OnCommandExecuting(System.Data.IDbCommand cmd)
        {
            if (cmd.CommandType == CommandType.Text)
			{
				dynamic ocmd = cmd;
				ocmd.BindByName = true;
			}

        }

       public override DBType ProviderType
       {
           get { return DBType.Oracle;}
       }

       public override LastInsertId ExecuteInsert(SqlStatement sql, string idKey)
        {
            IDbDataParameter param=null;
            if (!string.IsNullOrEmpty(idKey))
            {
             sql.Sql += string.Format(" returning {0} into :newid", EscapeName(idKey));
             var cmd = sql.Command;
            param = cmd.CreateParameter();
            param.ParameterName = ":newid";
            param.Value = DBNull.Value;
            param.Direction = ParameterDirection.ReturnValue;
            param.DbType = DbType.Int64;
            cmd.Parameters.Add(param);
            
            }
            using (sql)
            {
                sql.Execute();
                if (param == null) return LastInsertId.Empty;
                return new LastInsertId(param.Value);
            }

            
          
        }

        public override void MakePaged(string sql, out string selecSql, out string countSql)
        {
            throw new System.NotImplementedException();
        }

        public override string ParamPrefix
        {
            get { return ":"; }
        }
    }
}