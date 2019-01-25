using System;
using System.Collections.Generic;
using System.IO;

namespace Lab1
{
     class Program {
        protected static Dictionary<Tuple<Subject, string>, Dictionary<Object, HashSet<Right>>> Matrix;

        protected static HashSet<Object> Objects;

       
        public static string GetMatrixString()
        {
            var str = "";
            foreach (var pair in Matrix)
            {
                str += $"{pair.Key.Item1.Id}:{Environment.NewLine}";
                foreach (var valuePair in pair.Value)
                {
                    str += $"\t{valuePair.Key.Id}: ";
                    foreach (var right in valuePair.Value)
                    {
                        str += $"{right}, ";
                    }
                    str += Environment.NewLine;
                }
            }
            return str;
        }

        public static void GetMatrix()
        {
            var stream = new StreamReader("Matrix.txt");
            var lines = stream.ReadToEnd().Split(new[] { ":\r\n" }, StringSplitOptions.None);
            if (lines.Length == 0 || lines[0].Length <= 1) return;
            for (int i = 0; i < lines.Length; i++)
            {
                
                var items = lines[i].Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                switch (items.Length)
                {
                    case 0:
                        break;                
                    case 1:
                        //AddSubject(Userbyname(items[0]));
                        break;
                    default:
                        //AddSubject(Userbyname(items[0]));
                        for (int k = 1; k < items.Length; k++)
                        {
                            var items2 = items[k].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            switch (items2.Length)
                            {
                                case 0:
                                    break;
                                case 1:
                                    AddObject(new Object(items2[0]));
                                    break;
                                default:
                                    var obj = new Object(items2[0]);
                                    AddObject(obj);
                                    for (int j = 1; j < items2.Length; j++)
                                    {
                                        Right r;
                                        switch (items2[j])
                                        {
                                            case "Read":
                                                r = Right.Read;
                                                break;
                                            case "Write":
                                                r = Right.Write;
                                                break;
                                            case "Own":
                                                r = Right.Own;
                                                break;
                                            default: throw new Exception("wrong right");
                                        }
                                        AddRight(Userbyname(items[0]), obj, r);
                                    }
                                    break;
                            }
                        }
                        
                        break;
                }
                //if (items[1].Equals(String.Empty))
                //    Matrix.Add(UserPass(items[0], ""), new Dictionary<Object, HashSet<Right>>());
                //else Matrix.Add(UserPass(items[0], items[1]), new Dictionary<Object, HashSet<Right>>());
            }
            stream.Close();
        }
        public static void SaveMatrix()
        {
            var stream = new StreamWriter("Matrix.txt");
            foreach (var user in Matrix)
            {
                stream.Write(user.Key.Item1.Id + "|");
                foreach (var item in user.Value)
                {

                    stream.Write(item.Key.Id + ",");
                    foreach (var item2 in item.Value)
                    {
                        stream.Write(item2 + ",");
                    }
                    stream.Write("|");
                }
                stream.Write(":\r\n");
            }
            stream.Close();
        }
        public static Tuple<Subject, string> UserPass(string s1, string s2, string s3)
        {
            var subj = new Subject(s1);
            subj.admin = bool.Parse(s3);
            return new Tuple<Subject, string>(subj, s2);
        }
        public static bool Login(out bool admin, out Tuple<Subject, string> user)
        {
            Console.WriteLine("Enter login");
            var name = Console.ReadLine();
            Console.WriteLine("Password");
            var password = Console.ReadLine();
            foreach (var key in Matrix.Keys)
            {
                if (key.Item1.Id.Equals(name) && key.Item2.Equals(password))
                {
                    user = Userbyname(name);
                    admin = user.Item1.admin;
                    return true;
                }
            }
            user = null;
            admin = false;
            return false;
        }
        public static Tuple<Subject, string> Userbyname(string s)
        {
            foreach (var key in Matrix.Keys)
            {
                if (key.Item1.Id.Equals(s))
                {   
                    return key;
                }
            }
            return null;
        }
        public static bool Containskey(string s)
        {
            foreach (var key in Matrix.Keys)
            {
                if (key.Item1.Id.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }
        public static void Message()
        {
            Console.WriteLine("1-Add right\r\n2-Remove right\r\n3-Add subject\r\n4-Add object\r\n5-Remove subject\r\n6-Remove object\r\n7-Add several right for user to one object\r\n8-Delete several users\r\n9-Access check\r\nShow matrix\r\nLogout-Change user\r\nShutdown");
        }
        private static void Main(string[] args) 
        {
            Matrix = new Dictionary<Tuple<Subject, string>, Dictionary<Object, HashSet<Right>>>();
            Objects = new HashSet<Object>();
            GetUsers();
            Message();
            GetMatrix();
            var oper = Console.ReadLine().ToLowerInvariant();
            bool login = false;
            bool admin = false;
            var user1 = new Tuple<Subject,string>(new Subject(""),"");
            Right r;
            while (oper != "shutdown")
            {
                while (!login)
                {
                    login=Login(out admin, out user1);
                }
                switch (oper)
                {
                    case "1":
                        if (admin)
                        {
                            Console.WriteLine("User");
                            var user = Console.ReadLine();
                            Console.WriteLine("Object");
                            var obj10 = Console.ReadLine();
                            Console.WriteLine("Right");
                            var s = Console.ReadLine();
                            switch (s)
                            {
                                case "r":
                                    r = Right.Read;
                                    break;
                                case "w":
                                    r = Right.Write;
                                    break;
                                case "o":
                                    r = Right.Own;
                                    break;
                                default: throw new Exception("wrong right");
                            }
                            AddRight(Userbyname(user), new Object(obj10), r);
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "2":
                        if (admin)
                        {
                            Console.WriteLine("User");
                            var user = Console.ReadLine();
                            Console.WriteLine("Object");
                            var obj1 = Console.ReadLine();
                            Console.WriteLine("Right");
                            var s = Console.ReadLine();
                            switch (s)
                            {
                                case "r":
                                    r = Right.Read;
                                    break;
                                case "w":
                                    r = Right.Write;
                                    break;
                                case "o":
                                    r = Right.Own;
                                    break;
                                default: throw new Exception("wrong right");
                            }
                            RemoveRight(Userbyname(user), new Object(obj1), r);
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "3":
                        if (admin)
                        {
                            Console.WriteLine("Set Login");
                            var user = Console.ReadLine();
                            Console.WriteLine("Set Password");
                            var password = Console.ReadLine();
                            Console.WriteLine("Is Admin (true, false)");
                            var admin1 = Console.ReadLine();
                            AddSubject(UserPass(user,password, admin1));
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "4":
                        Console.WriteLine("Set Name");
                        var obj = Console.ReadLine();
                        AddObject(new Object(obj));
                        break;
                    case "5":
                        if (admin)
                        {
                            Console.WriteLine("Enter Login");
                            var user = Console.ReadLine();
                            RemoveSubject(Userbyname(user));
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "6":
                        if (admin)
                        {
                            Console.WriteLine("Set Name");
                            var obj2 = Console.ReadLine();
                            RemoveObject(new Object(obj2));
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "7":
                        if (admin)
                        {
                            Console.WriteLine("Enter END when you want to exit command");
                            Console.WriteLine("User");
                            var user = Console.ReadLine();
                            Console.WriteLine("Object");
                            var obj9 = Console.ReadLine();
                            Console.WriteLine("Right");
                            var s = Console.ReadLine().ToLowerInvariant();
                            while (!s.Equals("end"))
                            {
                                switch (s)
                                {
                                    case "r":
                                        r = Right.Read;
                                        break;
                                    case "w":
                                        r = Right.Write;
                                        break;
                                    case "o":
                                        r = Right.Own;
                                        break;
                                    default: throw new Exception("wrong right");
                                }
                                AddRight(Userbyname(user), new Object(obj9), r);
                                Console.WriteLine("Right");
                                s = Console.ReadLine().ToLowerInvariant();
                            }
                           
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "8":
                        if (admin)
                        {
                            Console.WriteLine("Enter END when you want to exit command");
                            Console.WriteLine("Enter Login");
                            var user = Console.ReadLine();
                            while (!user.Equals("end")) {
                                Console.WriteLine("Enter Login");
                                var current = Userbyname(user);
                                if (!current.Item1.admin)
                                {
                                    RemoveSubject(current);
                                }
                                user = Console.ReadLine();
                            }
                        }
                        else Console.WriteLine("Not admin");
                        break;
                    case "9":
                        Console.WriteLine("Object");
                        var obj0 = Console.ReadLine();
                        Console.WriteLine("Operation(r, w, o)");
                        var right = Console.ReadLine();
                        switch (right)
                        {
                            case "r":
                                r = Right.Read;
                                break;
                            case "w":
                                r = Right.Write;
                                break;
                            case "o":
                                r = Right.Own;
                                break;
                            default: throw new Exception("wrong right");
                        }
                        bool res = false;
                        foreach (var user in Matrix)
                        {
                            if (user.Key.Item1.Id.Equals(user1.Item1.Id))
                            foreach (var item in user.Value)
                            {
                                if (item.Key.Id.Equals(obj0))
                                foreach (var item2 in item.Value)
                                {
                                            if (item2.Equals(r)) res =true;
                                }
                            }
                        }
                        if (res)
                        {
                            Console.WriteLine("Accepted");
                        }
                        else
                        {
                            Console.WriteLine("Access denied");
                        }
                        break;
                    case "show":
                        Console.WriteLine(GetMatrixString());
                        break;
                    case "logout":
                        login = false;
                        Console.Clear();
                        Message();
                        break;
                    default: Console.WriteLine("Unknown command"); break;
                }
                oper = Console.ReadLine().ToLowerInvariant();
            }
            SaveUsers();
            SaveMatrix();
        }

        public static void GetUsers()
        {
            var userstream = new StreamReader("UserList.txt");
            var lines = userstream.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0 || lines[0].Length<=1) return;
            var d = new Dictionary<Object, HashSet<Right>>();
            for (int i = 0; i < lines.Length; i++)
            { 
                var items = lines[i].Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                if (items[1].Equals(String.Empty))
                    Matrix.Add(UserPass(items[0],"",items[2]), new Dictionary<Object, HashSet<Right>>());
                else Matrix.Add(UserPass(items[0],items[1],items[2]), new Dictionary<Object, HashSet<Right>>());
            }
            userstream.Close();
        }
        public static void SaveUsers()
        {
            var userstream = new StreamWriter("UserList.txt");
            foreach (var user in Matrix)
            {
                userstream.Write(user.Key.Item1.Id + " " + user.Key.Item2+ " " + user.Key.Item1.admin+"\r\n");
            }
            userstream.Close();
        }

        public static bool ContainsObject(Object obj)
        {
            foreach (var value in Matrix.Values)
            {
                foreach (var v in value)
                {
                        if (v.Key.Id.Equals(obj.Id))
                        {
                            return true;
                        }
                }
            }
            return false;
        }
        protected static void AddObject(Object obj)
        {
            if (ContainsObject(obj))
            {
                return;
            }
            //Objects.Add(obj);
            int i = 0;
            foreach (var subj in Matrix.Keys)
            {
                Matrix[subj].Add(new Object(obj.Id), new HashSet<Right>());
                ////i++;
                ////if (i == 1) break;
            }
        }

        protected static void AddRight(Tuple<Subject, string> subj, Object obj, Right right)
        {
            var subj1 = Userbyname(subj.Item1.Id);
            Check(subj, obj);
            foreach (var od in Matrix[subj1])
            {
                if (od.Key.Id.Equals(obj.Id))
                {
                    od.Value.Add(right);
                }
            }
        }

        protected static void AddSubject(Tuple<Subject, string> subj)
        {
            //if (Matrix.ContainsKey(subj))
            //{
            //    return;
            //}
            if (Objects.Contains(subj.Item1))
            {
                return;
            }
            Objects.Add(subj.Item1);
            Matrix.Add(subj, new Dictionary<Object, HashSet<Right>>());
        }

        protected static bool ApplyRule(Tuple<Subject, string> subj, Object obj, Right right)
        {
            Check(subj, obj);
            return Matrix[subj][obj].Contains(right);
        }

        protected static void Check(Tuple<Subject, string> subj, Object obj)
        {
            if (!Containskey(subj.Item1.Id))
            {
                throw new SubjectNotFound(subj.Item1);
            }
            //if (!Matrix[Userbyname(subj.Item1.Id)].ContainsKey(obj))
            //{
            //    throw new ObjectNotFound(obj);
            //}
        }

        protected static void RemoveObject(Object obj)
        {
            if (obj is Subject)
            {
                
                if (!Matrix.ContainsKey(Userbyname(obj.Id)))
                {
                    return;
                }
            }

            foreach (var subj in Matrix)
            {
                //Matrix[subj.Key].Remove(obj);
                foreach (var obj1 in subj.Value) {
                    if (obj1.Key.Id.Equals(obj.Id))
                    {
                        subj.Value.Remove(obj1.Key);
                        break;
                    }
                }

            }
            Objects.Remove(obj);
        }

        protected static void RemoveRight(Tuple<Subject, string> subj, Object obj, Right right)
        {
            var subj1 = Userbyname(subj.Item1.Id);
            Check(subj, obj);
            foreach (var od in Matrix[subj1])
            {
                if (od.Key.Id.Equals(obj.Id))
                {
                    od.Value.Remove(right);
                }
            }
        }

        protected static void RemoveSubject(Tuple<Subject, string> subj)
        {
            Objects.Remove(subj.Item1);
            Matrix.Remove(subj);
            foreach (var k in Matrix)
            {
                Matrix[k.Key].Remove(subj.Item1);
            }
        }
    }
}