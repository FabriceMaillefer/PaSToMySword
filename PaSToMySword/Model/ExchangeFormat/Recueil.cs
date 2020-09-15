using System.Collections.Generic;

namespace PaSToMySword.Model.ExchangeFormat
{
    public class Recueil
    {
        public string Id { get; set; }
        public string Titre { get; set; }
        public int Annee { get; set; }
        public IEnumerable<string> CommentairesId { get; set; } = new List<string>();
    }

    public class RecueilExchange
    {
        public IEnumerable<Recueil> Recueils { get; set; } = new List<Recueil>();
        public IEnumerable<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
    }
}