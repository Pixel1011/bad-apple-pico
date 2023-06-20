namespace XBMtoBIN
{
    internal class Program
    {
        static string path = "C:\\Users\\Pixel\\Downloads\\bap";
        static void Main(string[] args)
        {
            // menu
            Console.WriteLine("1. Convert XBM to BIN");
            Console.WriteLine("2. Assemble BIN files");
            Console.WriteLine("3. Convert XBM to BIN and assemble");
            Console.WriteLine("4. Verify Assembled bin");
            Console.WriteLine("5. Exit");
            char cho = Console.ReadKey().KeyChar;

            switch (cho) {
                case '1':
                    ConvertXBMToBIN();
                    break;
                case '2':
                    BytesAssemble();
                    break;
                case '3':
                    ConvertXBMToBIN();
                    BytesAssemble();
                    break;
                case '4':
                    VerifyFile();
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

        }

        // call after converting to bin
        // will combine all files to a single bin file
        static void BytesAssemble()
        {
            // this is stupid and dumb and stupid but my brain is working on 2 hours of sleep
            // for loop would be great considering its just linear but this somehow makes it easier to think
            string[] fies = Directory.GetFiles(path, "*.bin");
            List<string> files = fies.ToList();
            files.Remove("Complete");
            List<int> fileints = new List<int>();
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]).Replace("out", "");
                fileints.Add(int.Parse(files[i]));
            }
            fileints.Sort();

            List<byte> FinalBytes = new List<byte>();

            for (int i = 0; i < fileints.Count; i++)
            {
                byte[] bytes = File.ReadAllBytes(path + "\\out" + fileints[i] + ".bin");
                FinalBytes.AddRange(bytes);
                Console.WriteLine("Adding " + fileints[i] + " to final bytes");
                // then should just be able to read 1026 bytes at a time and maybe faster?
            }
            File.WriteAllBytes(path + "\\Complete.bin", FinalBytes.ToArray());
        }

        static void ConvertXBMToBIN()
        {
            string[] files = Directory.GetFiles(path, "*.xbm");
            // loop through each file
            foreach (string file in files)
            {
                // get the file name
                string fileName = Path.GetFileName(file);
                // get the file name without the extension
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                string normNameWithoutExtension = "out" + int.Parse(fileNameWithoutExtension.Split("out")[1]);
                // read the file
                string[] lines = File.ReadAllLines(file);
                // create a new file

                // split the line
                byte width, height;
                byte[] data;

                width = ((byte)int.Parse(lines[0].Split("image_width ")[1]));
                height = ((byte)int.Parse(lines[1].Split("image_height ")[1]));

                string CSHex = string.Join("", lines);
                CSHex = CSHex.Split("{")[1].Replace("};", "").Replace(",", "").Replace("0x", "").Replace(" ", "");

                data = convertData(CSHex);
                // convert CSHex to bytes
                // CSHex is now FFFFFFFFFFFF etc - and has an equal number of bytes
                byte[] completeArray = new byte[data.Length + 2];
                completeArray[0] = width;
                completeArray[1] = height;
                data.CopyTo(completeArray, 2);



                FileStream fs = File.Create(path + "\\" + normNameWithoutExtension + ".bin");
                fs.Write(completeArray, 0, completeArray.Length);
                fs.Close();

                File.Delete(file);
                Console.WriteLine("Converted " + fileName);
            }
        }

        static bool VerifyFile()
        {
            byte[] filebytes = File.ReadAllBytes(path + "\\Complete.bin");
            int w, h;
            w = filebytes[0];
            h = filebytes[1];
            int imglen = ((w * h) >> 3) + 2;
            
            for (int i = 0; i < filebytes.Length; i += imglen)
            {
                int fw, fh;
                fw = filebytes[i];
                fh = filebytes[i + 1];
                if (fw != w || fh != h)
                {
                    Console.WriteLine("Error on frame: " + i / imglen);
                }                
            }
            return true;
        }


        static byte[] convertData(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = (Convert.ToByte(hex.Substring(i, 2), 16));
            }
            return bytes;
        }
    }
}