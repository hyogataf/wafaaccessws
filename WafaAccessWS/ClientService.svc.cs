using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using WafaAccessWS.Models;
using System.Threading.Tasks;
using WafaAccessWS.Utils;
using System.Globalization;

namespace WafaAccessWS
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    [ServiceBehavior(Namespace = Constantes.Namespace)]
    public class ClientService : IClientService
    {
        public Client GetClientInfo(string login, string filialeId, string ribCompte, string timestamp, string wsSignature)
        {
            //On instancie le repository
            WAFAAuditlogRepository WAFAAuditlogdb = new WAFAAuditlogRepository();

            try
            {
                Debug.WriteLine("values recues : timestamp= " + timestamp + ", ribCompte= " + ribCompte + ", login= " + login + ", filialeId= " + filialeId + ", timestamp= " + timestamp);
                 
                //TODO check if wsSignature valid base64

               //on verifie que le timestamp est correct
               string dateformat = "yyyyMMddHHmm";
                DateTime dateTime;
                if (string.IsNullOrWhiteSpace(timestamp) || DateTime.TryParseExact(timestamp, dateformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime)==false) {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "99";
                    ClientError.returnMessage = "Timestamp absent ou incorrect";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                }


                //on verifie que filialeid est correct 
                if (string.IsNullOrWhiteSpace(filialeId))
                {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "99";
                    ClientError.returnMessage = "Filiale absente";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                } 

                //on verifie le rib.
                //il est obligatoire, et doit avoir au moins 23 caracteres
                if (string.IsNullOrWhiteSpace(ribCompte))
                {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "01";
                    ClientError.returnMessage = "Le rib est manquant";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                }
                if (ribCompte.Length < 23)
                {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "02";
                    ClientError.returnMessage = "Rib taille inférieure à 23 digits";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                }

                //on verifie la presence du login
                if (string.IsNullOrWhiteSpace(login) || login == "null")
                {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "10";
                    ClientError.returnMessage = "Login invalide";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                }
                /*
                 * on verifie ensuite la signature
                 */
                //on concatene les valeurs reçues (sauf la signature)
                String dataSeed = timestamp + "+" + ribCompte + "+" + login + "+" + filialeId;
                //on signe le string trouvé
                String wsSignatureGenerated = ToolsService.createSignature(dataSeed);
                //on compare notre signature avec celle envoyée
                bool verif = ToolsService.Verify(wsSignatureGenerated, wsSignature);
                if (verif == false)
                {
                    var ClientError = new Client();
                    ClientError.returnCode = "1";
                    ClientError.errorCode = "10";
                    ClientError.returnMessage = "Signature invalide";

                    //On declenche une tache de sauvegarde de l'action en parallele
                    Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                    return ClientError;
                }
                else
                {
                    //On instancie le repository
                    ClientEntityRepository db = new ClientEntityRepository();

                    var ClientEntity = db.Find(ribCompte);
                    if (ClientEntity != null)
                    {
                        //On declenche une tache de sauvegarde de l'action en parallele
                        Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientEntity.p_errorCode, ClientEntity.p_returnCode, ClientEntity.p_returnMessage); });

                        return TranslateClientEntityToClient(ClientEntity);
                    }
                    else
                    {
                        var ClientError = new Client();
                        ClientError.returnCode = "1";
                        ClientError.errorCode = "99";
                        ClientError.returnMessage = "Probleme inattendu survenu";

                        //On declenche une tache de sauvegarde de l'action en parallele
                        Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                        return ClientError;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetClientInfo error = " + ex.StackTrace);

                var ClientError = new Client();
                ClientError.returnCode = "1";
                ClientError.errorCode = "99";
                ClientError.returnMessage = "Probleme inattendu survenu";

                //On declenche une tache de sauvegarde de l'action en parallele
                Task.Factory.StartNew(() => { WAFAAuditlogdb.Create("READ", null, login, filialeId, ribCompte, timestamp, wsSignature, ClientError.errorCode, 1, ClientError.returnMessage); });

                return ClientError;
            }
        }


        private Client TranslateClientEntityToClient(ClientEntity ClientEntity)
        {
            Client clientInstance = new Client();

            clientInstance.cin = ClientEntity.p_cin;
            clientInstance.sigle = ClientEntity.p_sigle;
            clientInstance.nom = ClientEntity.p_nom;
            clientInstance.prenom = ClientEntity.p_prenom;
            clientInstance.dateNaissance = ClientEntity.p_dateNaissance.ToString();
            clientInstance.profession = ClientEntity.p_codeProfession;
            clientInstance.adresse1 = ClientEntity.p_adresse1;
            clientInstance.adresse2 = ClientEntity.p_adresse2;
            clientInstance.adresse3 = ClientEntity.p_adresse3;
            clientInstance.ville = ClientEntity.p_ville;
            clientInstance.pays = ClientEntity.p_pays;
            clientInstance.codePays = ClientEntity.p_codePays;
            clientInstance.sitFamiliale = ClientEntity.p_sitFamiliale;
            clientInstance.nationalite = ClientEntity.p_nationalite;
            clientInstance.compteBancaire = ClientEntity.p_compteBancaire;
            clientInstance.etatCompte = ClientEntity.p_etatCompte.ToString();
            clientInstance.returnCode = ClientEntity.p_returnCode.ToString();
            clientInstance.errorCode = ClientEntity.p_errorCode;
            clientInstance.returnMessage = ClientEntity.p_returnMessage;

            return clientInstance;
        }






        //methode juste pr tester la signature
        public Client GetClientInfo1(string login, string filialeId, string ribCompte, string timestamp, string wsSignature)
        {
            Debug.WriteLine("values recues : timestamp= " + timestamp + ", ribCompte= " + ribCompte + ", login= " + login + ", filialeId= " + filialeId + ", timestamp= " + timestamp);

            String dataSeed = timestamp + "+" + ribCompte + "+" + login + "+" + filialeId;
            String wsSignatureGenerated = ToolsService.createSignature(dataSeed);

            Debug.WriteLine("wsSignatureGenerated = " + wsSignatureGenerated);
            Debug.WriteLine("wsSignature recu = " + wsSignature);

            var signed = ToolsService.Sign(wsSignature);
            Debug.WriteLine("signed recu = " + signed);

            bool verif = ToolsService.Verify(wsSignatureGenerated, signed);




            Debug.WriteLine("wsSignature verif = " + verif);


            var ClientError = new Client();
            ClientError.returnCode = "1";
            ClientError.errorCode = "99";
            ClientError.returnMessage = "Probleme inattendu survenu";

            return ClientError;
        }

    }
}
