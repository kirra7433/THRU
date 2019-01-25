using System;
using System.Collections.Generic;
using System.IO;

namespace THRU
{   
    class Program
    {
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
                    str += $"\t{valuePair.Key.Id}.{valuePair.Key.Type}: ";
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
            var lines = stream.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0 || lines[0].Length <= 1) return;
            for (int i = 0; i < lines.Length; i++)
            {

                var items = lines[i].Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
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
                            var items3 = items2[0].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                            switch (items2.Length)
                            {
                                case 0:
                                    break;
                                case 1:
                                    AddObject(new Object(items3[0]),items3[1]);
                                    break;
                                default:
                                    var obj = new Object(items3[0]);
                                    AddObject(obj,items3[1]);
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
                                            default: throw new Exception("Неправильное право!");
                                        }
                                        AddRight(Userbyname(items[0]), obj, r);
                                    }
                                    break;
                            }
                        }

                        break;
                }
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

                    stream.Write(item.Key.Id+"."+item.Key.Type + ",");
                    foreach (var item2 in item.Value)
                    {
                        stream.Write(item2 + ",");
                    }
                    stream.Write("|");
                }
                stream.Write("\r\n");
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
            Console.WriteLine("Введите логин:");
            var name = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
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
            Console.WriteLine("1 - Добавить право\r\n" + "2 - Удалить право\r\n" + "3 - Добавить субъект\r\n" + "4 - Добавить объект\r\n" + "5 - Удалить субъект\r\n" + "6 - Удалить объект\r\n" + "7 - Проверить доступ\r\n" + "8 - Показать матрицу доступов\r\n" + "9 - Сменить пользователя\r\n" + "10 - Выключить\r\n" + "11 - Добавить несколько прав для пользователя");
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
            var user1 = new Tuple<Subject, string>(new Subject(""), "");
            Right r;
            while (oper != "10")
            {
                while (!login)
                {
                    login = Login(out admin, out user1);
                }
                switch (oper)
                {
                    case "1":
                        if (admin)
                        {
                            Console.WriteLine("\nПользователь:");
                            var user = Console.ReadLine();
                            Console.WriteLine("\nОбъект:");
                            var obj10 = Console.ReadLine();
                            Console.WriteLine("\nПраво:");
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
                                default: throw new Exception("\nНеправильное право!");
                            }
                            AddRight(Userbyname(user), new Object(obj10), r);
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    case "2":
                        if (admin)
                        {
                            Console.WriteLine("\nПользователь:");
                            var user = Console.ReadLine();
                            Console.WriteLine("\nОбъект:");
                            var obj1 = Console.ReadLine();
                            Console.WriteLine("\nПраво:");
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
                                default: throw new Exception("\nНеправильное право!");
                            }
                            RemoveRight(Userbyname(user), new Object(obj1), r);
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    case "3":
                        if (admin)
                        {
                            Console.WriteLine("\nВведите логин:");
                            var user = Console.ReadLine();
                            Console.WriteLine("\nВведите пароль");
                            var password = Console.ReadLine();
                            Console.WriteLine("\nВведите true, если администратор, и false, если нет");
                            var admin1 = Console.ReadLine();
                            AddSubject(UserPass(user, password, admin1));
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    case "4":
                        Console.WriteLine("\nИмя объекта:");
                        var obj = Console.ReadLine();
                        Console.WriteLine("\nТип объекта:");
                        var type = Console.ReadLine();
                        AddObject(new Object(obj),type);
                        break;
                    case "5":
                        if (admin)
                        {
                            Console.WriteLine("\nВведите логин:");
                            var user = Console.ReadLine();
                            RemoveSubject(Userbyname(user));
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    case "6":
                        if (admin)
                        {
                            Console.WriteLine("\nОбъект для удаления:");
                            var obj2 = Console.ReadLine();
                            RemoveObject(new Object(obj2));
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    
                    case "7":
                        Console.WriteLine("\nОбъект:");
                        var obj0 = Console.ReadLine();
                        Console.WriteLine("\nОперация (r, w, o)");
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
                            default: throw new Exception("\nНеправильное право!");
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
                                            if (item2.Equals(r)) res = true;
                                        }
                                }
                        }
                        if (res)
                        {
                            Console.WriteLine("\nОткрыто");
                        }
                        else
                        {
                            Console.WriteLine("\nДоступ закрыт");
                        }
                        break;
                    case "8":
                        Console.WriteLine(GetMatrixString());
                        break;
                    case "9":
                        login = false;
                        Console.Clear();
                        Message();
                        break;
                    case "11":
                        if (admin)
                        {
                            Console.WriteLine("\nВведите stop, если хотите закончить выполнение");
                            Console.WriteLine("\nПользователь:");
                            var user = Console.ReadLine();
                            Console.WriteLine("\nОбъект:");
                            var obj9 = Console.ReadLine();
                            Console.WriteLine("\nПраво:");
                            var s = Console.ReadLine().ToLowerInvariant();
                            while (!s.Equals("stop"))
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
                                    default: throw new Exception("\nНеправильное право!");
                                }
                                AddRight(Userbyname(user), new Object(obj9), r);
                                Console.WriteLine("\nПраво:");
                                s = Console.ReadLine().ToLowerInvariant();
                            }

                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    case "12":
                        if (admin)
                        {
                            Console.WriteLine("\nВведите stop, если хотите закончить выполнение");
                            Console.WriteLine("\nВведите логин:");
                            var user = Console.ReadLine();
                            while (!user.Equals("stop"))
                            {
                                Console.WriteLine("\nВведите логин:");
                                var current = Userbyname(user);
                                if (!current.Item1.admin)
                                {
                                    RemoveSubject(current);
                                }
                                user = Console.ReadLine();
                            }
                        }
                        else Console.WriteLine("\nВы не администратор!");
                        break;
                    default: Console.WriteLine("Неизвестная команда!"); break;
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
            if (lines.Length == 0 || lines[0].Length <= 1) return;
            var d = new Dictionary<Object, HashSet<Right>>();
            for (int i = 0; i < lines.Length; i++)
            {
                var items = lines[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (items[1].Equals(String.Empty))
                {
                    var s = UserPass(items[0], "", items[2]);
                    s.Item1.Type = ObjectType.user;
                    Matrix.Add(s, new Dictionary<Object, HashSet<Right>>());
                }
                else
                {
                    var s = UserPass(items[0], items[1], items[2]);
                    s.Item1.Type = ObjectType.user;
                    Matrix.Add(s, new Dictionary<Object, HashSet<Right>>());
                }
            }
            userstream.Close();
        }
        public static void SaveUsers()
        {
            var userstream = new StreamWriter("UserList.txt");
            foreach (var user in Matrix)
            {
                userstream.Write(user.Key.Item1.Id + " " + user.Key.Item2 + " " + user.Key.Item1.admin + "\r\n");
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
        protected static void AddObject(Object obj, string type)
        {
            if (ContainsObject(obj))
            {
                return;
            }
            int i = 0;
            var r = new ObjectType();
            switch(type)
            {
                case "exe":
                    r = ObjectType.exe;
                    break;
                case "doc":
                    r = ObjectType.doc;
                    break;
                case "jpg":
                    r = ObjectType.jpg;
                    break;
            }
            foreach (var subj in Matrix.Keys)
            {
                var o = new Object(obj.Id);
                o.Type = r;
                Matrix[subj].Add(o, new HashSet<Right>());
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
            if (Objects.Contains(subj.Item1))
            {
                return;
            }
            subj.Item1.Type = ObjectType.user;
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
                foreach (var obj1 in subj.Value)
                {
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