// See https://aka.ms/new-console-template for more information
using MyFirstProject;
using API_DiscoverAlgorithm;

Console.WriteLine("Hello, World!");

using (var db = new MyDbContext())
{
    aquireMovieDetails n = new aquireMovieDetails();


    n.getMovieDetails(9836, db);

}

// using (var context = new MyDbContext()) {
//   var kindergarten = new Kindergarten() {
//     Name = "Elefanterne",
//     Address = "Højbjerg",
//   };
//   context.Kindergartens.Add(kindergarten);
//   context.SaveChanges();
// }