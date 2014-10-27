using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WafaAccessWS.Models
{
    public class WAFAAuditlog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //  champ autogeneré dans la bdd
        [Column("WAFAAUDITLOGID")]
        public long WAFAAuditlogId { get; set; }

        [Column("ACTION")]
        public string Action { get; set; } //READ

        [Column("DATEACTION")]
        public DateTime? DateAction { get; set; }

        [Column("USERACTION")]
        public string UserAction { get; set; }

        [Column("LOGIN")]
        public string login { get; set; }

        [Column("FILIALEID")]
        public string filialeId { get; set; }

        [Column("RIBCOMPTE")]
        public string ribCompte { get; set; }

        [Column("TIMESTAMP")]
        public string timestamp { get; set; }

        [Column("WSSIGNATURE")]
        public string wsSignature { get; set; }

        [Column("RETURNCODE")]
        public int? returnCode { get; set; } //0: Action réalisée avec succès; 1:Action non réalisée

        [Column("ERRORCODE")]
        public string errorCode { get; set; } //99: Erreur technique; 10: Login ou Signature invalide; 01:Le rib est manquant; 02:Rib taille inférieure à 23 digits; 05:Pas de client pour ce compte

        [Column("RETURNMESSAGE")]
        public string returnMessage { get; set; } //length:60
    }
}
