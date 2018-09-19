using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAdventure
{
    public enum LevelNumber
    {
        level1,
        level2,
        level3,
        level4,
        level5
    }

    class LevelManager
    {
        private static Dictionary<LevelNumber, string> levelDict = new Dictionary<LevelNumber, string>() {
            { LevelNumber.level1, "sugar-adventure" },
            { LevelNumber.level2, "SugarAdventure3" },
            //{ LevelNumber.level3, "sugar-adventure" },
            //{ LevelNumber.level2, "next-level" },
        };

        private Level[] levels;
        public Level[] Levels
        {
            get
            {
                return levels;
            }
        }
        

        private string levelSource = "";

        public LevelManager()
        {
            levelSource = Path.GetFullPath("..\\..\\..\\..\\..\\..\\..\\data\\levels\\");
        }

        public void LoadContent()
        {
            Console.WriteLine("Loading levels");
            
            var levelArray = levelDict.Keys.ToArray();
            int levelCount = levelArray.Length;

            levels = new Level[levelCount];
            
            for(int i = 0; i < levelCount; i++)
            {
                string levelFolderName = levelArray[i].ToString();
                string levelFileName = "";
                levelDict.TryGetValue(levelArray[i], out levelFileName);

                levels[i] = new Level(levelSource, levelFolderName, levelFileName);
            }
        }

        public void SetSource(string path)
        {
            levelSource = path;
        }

        public Level GetLevel(LevelNumber _levelNumber)
        {
            return levels[getIndexOfKey(levelDict, _levelNumber)];
        }

        private int getIndexOfKey(Dictionary<LevelNumber, string> tempDict, LevelNumber key)
        {
            int index = -1;
            foreach (LevelNumber value in tempDict.Keys)
            {
                index++;
                if (key == value)
                    return index;
            }
            return -1;
        }

        public string GetSource()
        {
            return levelSource;
        }

        //private void Add(string levelName)
        //{
        //    levels.Add(new Level(levelSource, levelName));
        //}
    }
}
