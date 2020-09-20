using System.Collections.Generic;

namespace Common.Commentaries.Model
{
    public class Recueil
    {
        public string Id { get; set; }
        public string Titre { get; set; }
        public int Annee { get; set; }
        public List<string> CommentairesId { get; set; } = new List<string>();
    }

    public class RecueilExchange
    {
        public List<Recueil> Recueils { get; set; } = new List<Recueil>();
        public List<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
    }
}