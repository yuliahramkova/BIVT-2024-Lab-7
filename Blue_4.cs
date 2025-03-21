using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Lab_7;
public class Blue_4
{
    public abstract class Team
    {
        private string _name;
        private int[] _scores;

        public string Name => _name;
        public int[] Scores
        {
            get
            {
                if (_scores == null) 
                    return default(int[]);
                int[] newScores = new int[_scores.Length];
                for (int i = 0; i<_scores.Length; i++)
                    newScores[i] = _scores[i];
                return newScores;
            }
        }
        public int TotalScore
        {
            get
            {
                if (_scores == null || _scores.Length == 0)
                    return 0;
                int total = 0;
                foreach (int score in _scores)
                    total += score;
                return total;
            }
        }

        public Team(string name)
        {
            _name = name;
            _scores = new int[0];
        }

        public void PlayMatch(int result)
        {
            if (_scores == null) return;
            int[] newScores = new int[_scores.Length+1];
            Array.Copy(_scores, newScores, _scores.Length);
            newScores[_scores.Length] = result;
            _scores = newScores;
        }

        public void Print()
        {
            Write($"{_name}: {TotalScore}");
            WriteLine("");
        }
    }


    public class Group
    {
        private string _name;
        private ManTeam[] _manTeams;
        private WomanTeam[] _womanTeams;

        private int _countMen;
        private int _countWomen;

        public string Name => _name;
        public ManTeam[] ManTeams => _manTeams;
        public WomanTeam[] WomanTeams => _womanTeams;

        public Group(string name)
        {
            _name = name;
            _manTeams = new ManTeam[12];
            _womanTeams = new WomanTeam[12];
            _countMen = 0;
            _countWomen = 0;
        }

        public void Add(Team team)
        {
            if (team == null)
                return;
            
            if (team is ManTeam)
            {
                if (_manTeams == null || _countMen == 12 || team == null) 
                    return;
                else
                    _manTeams[_countMen++] = team as ManTeam;
            }
            else if (team is WomanTeam)
            {
                if (_womanTeams == null || _countWomen == 12 || team == null) 
                    return;
                else
                    _womanTeams[_countWomen++] = team as WomanTeam;
            }

        }
        public void Add(Team[] teams)
        {
            if (teams == null || teams.Length == 0)
                return;
            foreach (Team team in teams)
                Add(team);
        }
        private void SortGroup(Team[] teams)
        {
            if (teams == null || teams.Length < 2)
                return; 
            var sortedTeams = teams.OrderByDescending(team => team.TotalScore).ToArray();
            Array.Copy(sortedTeams, teams, teams.Length);
        }
        public void Sort()
        {
            SortGroup(_womanTeams);
            SortGroup(_manTeams);
        }
        public static Group Merge(Group group1, Group group2, int size)
        {
            if (size <= 0) return default(Group);
            group1.Sort();
            group2.Sort();
            Group manTeam = MergeTeam(group1.ManTeams, group2.ManTeams, group1.ManTeams.Length+group2.ManTeams.Length);
            Group womanTeam = MergeTeam(group1.WomanTeams, group2.WomanTeams, group1.WomanTeams.Length+group2.WomanTeams.Length);
            Group total = new Group("Финалисты");
            total.Add(manTeam.ManTeams);
            total.Add(womanTeam.WomanTeams);
            return total;
        }
        public static Group MergeTeam(Team[] group1, Team[] group2, int size)
        {
            if (size <= 0) return default(Group);
            Group result = new Group("Финалисты");
            int i = 0; int j = 0;
            while (i < size/2 && j < size/2)
            {
                if (group1[i].TotalScore >= group2[j].TotalScore)
                    result.Add(group1[i++]);
                else
                    result.Add(group2[j++]);
            }
            while (i < size/2)
            {
                result.Add(group1[i++]);
            }
            while (j < size/2)
            {
                result.Add(group2[j++]);
            }
            return result;
        }

        public void Print()
        {
            WriteLine($"{_name}: ");
            Write("Women teams: ");
            foreach (var team in _womanTeams)
            {
                team.Print();
            }
            Write("Men teams: ");
            foreach (var team in _manTeams)
            {
                team.Print();
            }
            WriteLine("");
        }
    }

    public class ManTeam : Team
    {
        public ManTeam(string name) : base(name) {}
    }

    public class WomanTeam : Team
    {
        public WomanTeam(string name) : base(name) {}
    }
}