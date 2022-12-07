using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace posts
{
    public class GuestBook
    {
        private string filename = @"guestbook.json";
        private List<Post> posts = new List<Post>();

        public GuestBook(){ 
            if(File.Exists(@"GuestBook.json")==true){ // Om json-filen finns använder vi den för att lagra data
                string jsonString = File.ReadAllText(filename);
                posts = JsonSerializer.Deserialize<List<Post>>(jsonString);
            }
        }
        public Post addPost(Post post){
            posts.Add(post);
            marshal();         
            return post;
        }
        
        public int delPost(int index){
            posts.RemoveAt(index);
            marshal();
            return index;
        }

        public List<Post> getPosts(){
            return posts;
        }

        private void marshal()
        {
            // Serialize all the objects and save to file
            var jsonString = JsonSerializer.Serialize(posts);
            File.WriteAllText(filename, jsonString);
        }
    }

    public class Post //Klass för att lägga till inlägg med författare och textinnehåll
    {

        private string post_author;
        private string post_text;    
        public string Post_author
        {
            set {this.post_author = value;}
            get {return this.post_author;}
        }    
        public string Post_text
        {
            set {this.post_text = value;}
            get {return this.post_text;}
        }
    }

    class Program //Klass för vad som sker när man kör programmet
    {
        static void Main(string[] args)
        {
 
            GuestBook guestbook = new GuestBook();
            int i=0;

            while(true){ //Skriver till konsolen och ger menyalternativ
                Console.Clear();
                Console.CursorVisible = false;
                Console.WriteLine("GÄSTBOK\n\n");

                Console.WriteLine("1. Skriv ett inlägg");
                Console.WriteLine("2. Ta bort ett inlägg\n");
                Console.WriteLine("X. Avsluta\n");

                i=0;
                foreach(Post post in guestbook.getPosts()){
                    Console.WriteLine("[" + i++ + "] " + post.Post_author + " - " + post.Post_text); //Skriver ut tidigare inlägg
                }

                int inp = (int) Console.ReadKey(true).Key;
                switch (inp) {
                    case '1': //Detta sker om man väljer menyalternativ "1"
                        Console.Clear();
                        Console.CursorVisible = true; 
                        Console.Write("Författare: ");
                        string author = Console.ReadLine();
                        Post obj = new Post();
                        obj.Post_author = author;
                        Console.Write("Ditt inlägg: ");
                        string text = Console.ReadLine();
                        obj.Post_text = text;
                        if(!String.IsNullOrEmpty(text) && !String.IsNullOrEmpty(author)) guestbook.addPost(obj); //Kontrollerar så att textfälten inte är tomma innan objektet lagras till fil
                        break;
                    case '2': 
                        Console.Clear();
                        Console.CursorVisible = true;
                        Console.Write("Ange index för det inlägg du vill radera: ");
                        string index = Console.ReadLine();
                        guestbook.delPost(Convert.ToInt32(index));
                        break;
                    case 88: 
                        Environment.Exit(0);
                        break;
                }
 
            }

        }
    }
}

