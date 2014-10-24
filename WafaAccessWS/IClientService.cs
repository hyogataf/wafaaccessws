using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WafaAccessWS
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract(Namespace = Constantes.Namespace)]
    public interface IClientService
    {
        [OperationContract]
        Client GetClientInfo(string login, string filialeId, string ribCompte, string timestamp, string wsSignature);

        // TODO: ajoutez vos opérations de service ici
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service
    [DataContract]
    public class Client
    {
        [DataMember]
        public string cin { get; set; } //?

        [DataMember]
        public string sigle { get; set; } //Titre: Mr, Mme, Mlle

        [DataMember]
        public string nom { get; set; }

        [DataMember]
        public string prenom { get; set; }

        [DataMember]
        public string dateNaissance { get; set; } //yyyy-MM-dd

        [DataMember]
        public string profession { get; set; }

        [DataMember]
        public string adresse1 { get; set; }

        [DataMember]
        public string adresse2 { get; set; }

        [DataMember]
        public string adresse3 { get; set; }

        [DataMember]
        public string ville { get; set; }

        [DataMember]
        public string pays { get; set; }

        [DataMember]
        public string codePays { get; set; } //Code ISO du pays (ex : 250 pour la France)

        [DataMember]
        public string sitFamiliale { get; set; } //MARIE, CELIBATAIRE?, VEUF?,

        [DataMember]
        public string nationalite { get; set; } //Francaise

        [DataMember]
        public string compteBancaire { get; set; } //Sur 11 digits

        [DataMember]
        public string etatCompte { get; set; } //0 : ouvert, 1 : en instance de fermeture, 2 : fermé

        [DataMember]
        public string returnCode { get; set; } //0: Action réalisée avec succès; 1:Action non réalisée

        [DataMember]
        public string errorCode { get; set; } //99: Erreur technique; 10: Login ou Signature invalide; 01:Le rib est manquant; 02:Rib taille inférieure à 23 digits; 05:Pas de client pour ce compte

        [DataMember]
        public string returnMessage { get; set; } //length:60

    }
}
