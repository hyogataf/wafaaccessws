using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WafaAccessWS.Models
{
    public class ClientEntity
    {
        public long? ClientEntityId { get; set; }

        [Column("P_CIN")]
        public string p_cin { get; set; } //?

        [Column("P_SIGLE")]
        public string p_sigle { get; set; } //Titre: Mr, Mme, Mlle

        [Column("P_NOM")]
        public string p_nom { get; set; }

        [Column("P_PRENOM")]
        public string p_prenom { get; set; }

        [Column("P_DATE_NAISSANCE")]
        public DateTime? p_dateNaissance { get; set; } //yyyy-MM-dd

        [Column("P_CODE_PROFESSION")]
        public string p_codeProfession { get; set; }

        [Column("P_ADRESSE1")]
        public string p_adresse1 { get; set; }

        [Column("P_ADRESSE2")]
        public string p_adresse2 { get; set; }

        [Column("P_ADRESSE3")]
        public string p_adresse3 { get; set; }

        [Column("P_VILLE")]
        public string p_ville { get; set; }

        [Column("P_PAYS")]
        public string p_pays { get; set; }

        [Column("P_CODE_PAYS")]
        public string p_codePays { get; set; } //Code ISO du pays (ex : 250 pour la France)

        [Column("P_SIT_FAMILIALE")]
        public string p_sitFamiliale { get; set; } //MARIE, CELIBATAIRE?, VEUF?,

        [Column("P_NATIONALITE")]
        public string p_nationalite { get; set; } //Francaise

        [Column("P_COMPTE_BANCAIRE")]
        public string p_compteBancaire { get; set; } //Sur 11 digits

        [Column("P_ETAT_COMPTE")]
        public int? p_etatCompte { get; set; } //0 : ouvert, 1 : en instance de fermeture, 2 : fermé

        [Column("P_RETURN_CODE")]
        public int? p_returnCode { get; set; } //0: Action réalisée avec succès; 1:Action non réalisée

        [Column("P_ERROR_CODE")]
        public string p_errorCode { get; set; } //99: Erreur technique; 10: Login ou Signature invalide; 01:Le rib est manquant; 02:Rib taille inférieure à 23 digits; 05:Pas de client pour ce compte

        [Column("p_return_msg")]
        public string p_returnMessage { get; set; } //length:60
    }
}
