using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using System.Data;
//using Oracle.DataAccess.Client;

namespace WafaAccessWS.Models
{
    public class ClientEntityRepository : IClientEntityRepository
    {
        WafaaccessContext context = new WafaaccessContext();

        public IQueryable<ClientEntity> All
        {
            get { return context.ClientEntity; }
        }


        public ClientEntity Find(string id)
        {
            try
            {
                context.Database.Connection.Open();
                var command = context.Database.Connection.CreateCommand();
                command.CommandText = "PKG_WAFA_ASSUR.getClientInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // command.BindByName = True; 

                //IN parameter de la procedure
                var InParamRib = new OracleParameter();
                InParamRib.ParameterName = "p_rib";
                InParamRib.Value = id;
                InParamRib.Direction = System.Data.ParameterDirection.Input;
                command.Parameters.Add(InParamRib);
                //  command.Parameters.Add("@P_RIB", id);

                //OUT parameters de la procedure 
                var OutParamCin = new OracleParameter("p_cin", OracleDbType.Varchar2);
                OutParamCin.Direction = System.Data.ParameterDirection.Output;
                OutParamCin.Size = 100;
                command.Parameters.Add(OutParamCin);

                var OutParamSigle = new OracleParameter("p_sigle", OracleDbType.Varchar2);
                OutParamSigle.Direction = System.Data.ParameterDirection.Output;
                OutParamSigle.Size = 100;
                command.Parameters.Add(OutParamSigle);

                var OutParamNom = new OracleParameter("p_nom", OracleDbType.Varchar2);
                OutParamNom.Direction = System.Data.ParameterDirection.Output;
                OutParamNom.Size = 100;
                command.Parameters.Add(OutParamNom);

                var OutParamPrenom = new OracleParameter("prenom", OracleDbType.Varchar2);
                OutParamPrenom.Direction = System.Data.ParameterDirection.Output;
                OutParamPrenom.Size = 100;
                command.Parameters.Add(OutParamPrenom);

                var OutParamDateNaissance = new OracleParameter("p_date_naissance", OracleDbType.Date);
                OutParamDateNaissance.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(OutParamDateNaissance);

                var OutParamCodeProfession = new OracleParameter("p_code_profession", OracleDbType.Varchar2);
                OutParamCodeProfession.Direction = System.Data.ParameterDirection.Output;
                OutParamCodeProfession.Size = 100;
                command.Parameters.Add(OutParamCodeProfession);

                var OutParamAdresse1 = new OracleParameter("p_adresse1", OracleDbType.Varchar2);
                OutParamAdresse1.Direction = System.Data.ParameterDirection.Output;
                OutParamAdresse1.Size = 100;
                command.Parameters.Add(OutParamAdresse1);

                var OutParamAdresse2 = new OracleParameter("p_adresse2", OracleDbType.Varchar2);
                OutParamAdresse2.Direction = System.Data.ParameterDirection.Output;
                OutParamAdresse2.Size = 100;
                command.Parameters.Add(OutParamAdresse2);

                var OutParamAdresse3 = new OracleParameter("p_adresse3", OracleDbType.Varchar2);
                OutParamAdresse3.Direction = System.Data.ParameterDirection.Output;
                OutParamAdresse3.Size = 100;
                command.Parameters.Add(OutParamAdresse3);

                var OutParamVille = new OracleParameter("p_ville", OracleDbType.Varchar2);
                OutParamVille.Direction = System.Data.ParameterDirection.Output;
                OutParamVille.Size = 100;
                command.Parameters.Add(OutParamVille);

                var OutParamPays = new OracleParameter("p_pays", OracleDbType.Varchar2);
                OutParamPays.Direction = System.Data.ParameterDirection.Output;
                OutParamPays.Size = 100;
                command.Parameters.Add(OutParamPays);

                var OutParamCodePays = new OracleParameter("p_code_pays", OracleDbType.Varchar2);
                OutParamCodePays.Direction = System.Data.ParameterDirection.Output;
                OutParamCodePays.Size = 100;
                command.Parameters.Add(OutParamCodePays);

                var OutParamSitFamiliale = new OracleParameter("p_sit_familiale", OracleDbType.Varchar2);
                OutParamSitFamiliale.Direction = System.Data.ParameterDirection.Output;
                OutParamSitFamiliale.Size = 100;
                command.Parameters.Add(OutParamSitFamiliale);

                var OutParamTelFixe = new OracleParameter("p_tel_fixe", OracleDbType.Varchar2);
                OutParamTelFixe.Direction = System.Data.ParameterDirection.Output;
                OutParamTelFixe.Size = 100;
                command.Parameters.Add(OutParamTelFixe);

                var OutParamTelGsm = new OracleParameter("p_tel_gsm", OracleDbType.Varchar2);
                OutParamTelGsm.Direction = System.Data.ParameterDirection.Output;
                OutParamTelGsm.Size = 100;
                command.Parameters.Add(OutParamTelGsm);

                var OutParamNationalite = new OracleParameter("p_nationalite", OracleDbType.Varchar2);
                OutParamNationalite.Direction = System.Data.ParameterDirection.Output;
                OutParamNationalite.Size = 100;
                command.Parameters.Add(OutParamNationalite);

                var OutParamCpteBancaire = new OracleParameter("p_compte_bancaire", OracleDbType.Varchar2);
                OutParamCpteBancaire.Direction = System.Data.ParameterDirection.Output;
                OutParamCpteBancaire.Size = 100;
                command.Parameters.Add(OutParamCpteBancaire);

                var OutParamEtatCompte = new OracleParameter("p_etat_compte", OracleDbType.Decimal);
                OutParamEtatCompte.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(OutParamEtatCompte);

                var OutParamReturnCode = new OracleParameter("p_return_code", OracleDbType.Decimal);
                OutParamReturnCode.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(OutParamReturnCode);

                var OutParamErrorCode = new OracleParameter("p_error_code", OracleDbType.Varchar2);
                OutParamErrorCode.Direction = System.Data.ParameterDirection.Output;
                OutParamErrorCode.Size = 100;
                command.Parameters.Add(OutParamErrorCode);

                var OutParamReturnMsg = new OracleParameter("p_return_msg", OracleDbType.Varchar2);
                OutParamReturnMsg.Direction = System.Data.ParameterDirection.Output;
                OutParamReturnMsg.Size = 100;
                command.Parameters.Add(OutParamReturnMsg);

                /*    var ReturnValue = new OracleParameter();
                    ReturnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                      command.Parameters.Add(ReturnValue);*/

                command.ExecuteNonQuery();
                //Debug.WriteLine("OutParamReturnMsg Value = " + OutParamReturnMsg.Value);
                //Debug.WriteLine("command OutParamReturnMsg = " + command.Parameters["P_RETURN_MSG"].Value);
                //Debug.WriteLine("command OutParamDateNaissance = " + OutParamDateNaissance.Value);

                //Construction du ClientEntity à retourner
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                var ClientEntity = new ClientEntity();
                ClientEntity.p_cin = OutParamCin.Value.ToString();
                ClientEntity.p_sigle = OutParamSigle.Value.ToString();
                ClientEntity.p_nom = OutParamNom.Value.ToString();
                ClientEntity.p_prenom = OutParamPrenom.Value.ToString();
                if (!string.IsNullOrEmpty(OutParamDateNaissance.Value.ToString()) && OutParamDateNaissance.Value.ToString() != "null")
                {
                    ClientEntity.p_dateNaissance = Convert.ToDateTime(OutParamDateNaissance.Value.ToString());
                }
                ClientEntity.p_codeProfession = OutParamCodeProfession.Value.ToString();
                ClientEntity.p_adresse1 = OutParamAdresse1.Value.ToString();
                ClientEntity.p_adresse2 = OutParamAdresse2.Value.ToString();
                ClientEntity.p_adresse3 = OutParamAdresse3.Value.ToString();
                ClientEntity.p_ville = OutParamVille.Value.ToString();
                ClientEntity.p_pays = OutParamPays.Value.ToString();
                ClientEntity.p_codePays = OutParamCodePays.Value.ToString();
                ClientEntity.p_sitFamiliale = OutParamSitFamiliale.Value.ToString();
                ClientEntity.p_nationalite = OutParamNationalite.Value.ToString();
                ClientEntity.p_compteBancaire = OutParamCpteBancaire.Value.ToString();
                int a;
                if (int.TryParse(OutParamEtatCompte.Value.ToString(), out a))
                {
                    ClientEntity.p_etatCompte = int.Parse(OutParamEtatCompte.Value.ToString());
                }
                if (int.TryParse(OutParamReturnCode.Value.ToString(), out a))
                {
                    ClientEntity.p_returnCode = int.Parse(OutParamReturnCode.Value.ToString());
                }
                else
                {
                    ClientEntity.p_returnCode = 1;// p_returnCode in [0, 1]. Si la valeur renvoyée est differente, on considere que l'operation a echoué ==1
                }
                ClientEntity.p_errorCode = OutParamErrorCode.Value.ToString();
                ClientEntity.p_returnMessage = OutParamReturnMsg.Value.ToString();

                return ClientEntity;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ex error == " + ex);
                throw;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }


    public interface IClientEntityRepository : IDisposable
    {
        IQueryable<ClientEntity> All { get; }
        //IQueryable<ClientEntity> AllIncluding(params Expression<Func<ClientEntity, object>>[] includeProperties);
        ClientEntity Find(string id);
        //DbSqlQuery<ClientEntity> FindAllByRequest(long Cicrequestid);
        //void InsertOrUpdate(ClientEntity ClientEntity);
        //void Delete(long id);
        //void Save();
    }
}
