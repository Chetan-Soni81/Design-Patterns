using System;
using System.ComponentModel;

namespace Exercise.BehavorialPatterns
{
    public abstract class DataProcessor
    {
        public void ProcessData()
        {
            Console.WriteLine($"\n=== Starting {GetProcessorName()} ===\n");

            var data = ReadData();

            if (ValidateData(data))
            {
                var processed = TransformData(data);
                var analyzed = AnalyzeData(processed);
                SaveResults(analyzed);

                if(ShouldGenerateReport())
                {
                    GenerateReport();
                }

                Console.WriteLine($"\n✅ {GetProcessorName()} completed successfully!");
            }
            else
            {
                Console.WriteLine($"\n❌ {GetProcessorName()} failed - invalid data!");
            }
        }

        protected abstract string GetProcessorName();
        protected abstract string ReadData();
        protected abstract string TransformData(string data);
        protected abstract string AnalyzeData(string data);

        protected bool ValidateData(string data)
        {
            Console.WriteLine($"📋 Validating data...");
            bool isValid = !string.IsNullOrEmpty(data);
            Console.WriteLine(isValid ? "✅ Data is valid" : "❌ Data is invalid");
            return isValid;
        }

        protected void SaveResults(string results)
        {
            Console.WriteLine($"💾 Saving Results: {results.Substring(0, Math.Min(50, results.Length))}...");
            Console.WriteLine($"✅ Results saved successfully");
        }

        protected virtual bool ShouldGenerateReport()
        {
            return false;
        }

        protected virtual void GenerateReport()
        {
            Console.WriteLine($"📊 Generate report...");
        }
    }

    public class CSVDataProcessor : DataProcessor
    {
        protected override string GetProcessorName()
        {
            return "CSV Data Processor";
        }

        protected override string ReadData()
        {
            Console.WriteLine($"📂 Reading CSV file...");
            string csvData = "Name,Age,City\nJohn,30,NYC\nJane,25,LA\nBob,35,Chicago";
            Console.WriteLine($"✅ Read {csvData.Split('\n').Length} rows");
            return csvData;
        }

        protected override string TransformData(string data)
        {
            Console.WriteLine($"🔁 Transforming CSV data to objects...");

            System.Threading.Thread.Sleep(500);
            string transformed = "Transformed: " + data.Replace(",", " | ");
            Console.WriteLine($"✅ Transformed complete");
            return transformed;
        }

        protected override string AnalyzeData(string data)
        {
            Console.WriteLine($"📊 Analyzing data statistics...");
            int recordCount = data.Split('\n').Length;
            string analysis = $"Analysis complete: {recordCount} records processed";
            Console.WriteLine($"✅ {analysis}");
            return analysis;
        }

        protected override bool ShouldGenerateReport()
        {
            return true;
        }

        protected override void GenerateReport()
        {
            Console.WriteLine($"📊 Generating CSV summary report...");
            Console.WriteLine($"   - Format: CSV");
            Console.WriteLine($"   - Charts: Bar chart, Line graph");
            Console.WriteLine($"✅ Report generated");
        }
    }

    public class JSONDataProcessor : DataProcessor
    {
        protected override string GetProcessorName()
        {
            return "JSON Data Processor";
        }

        protected override string ReadData()
        {
            Console.WriteLine($"📂 Reading JSON file...");
            string jsonData = "{\"users\":[\"name\":\"Alice\",\"Score\":95]}";
            Console.WriteLine($"✅ JSON file loaded");
            return jsonData;
        }

        protected override string TransformData(string data)
        {
            Console.WriteLine($"🔁 Parsing JSON structure...");
            System.Threading.Thread.Sleep(1000);
            string transformed = "Parsed: " + data.Replace("{", "[").Replace("}","]");
            Console.WriteLine($"✅ JSON parsed successfully");
            return transformed;
        }

        protected override string AnalyzeData(string data)
        {
            Console.WriteLine($"📊 Running JSON schema validation...");
            string analysis = "Schema validation passed, all fields present";
            Console.WriteLine($"✅ {analysis}");
            return analysis;
        }
    }

    public class XMLDataProcessor : DataProcessor
    {
        protected override string GetProcessorName()
        {
            return "XML Data Processor";
        }

        protected override string ReadData()
        {
            Console.WriteLine($"📂 Reading XML file...");
            string xmlData = "<root><item>Data</item></root>";
            Console.WriteLine($"✅ XML file loaded");
            return xmlData;
        }

        protected override string TransformData(string data)
        {
            Console.WriteLine($"🔁 Parsing XML DOM...");
            System.Threading.Thread.Sleep(400);
            string transformed = "DOM: " + data.Replace("<", "[").Replace(">", "]");
            Console.WriteLine($"✅ XML DOM created");
            return transformed;
        }

        protected override string AnalyzeData(string data)
        {
            Console.WriteLine($"📊 Validating XML agains XSD schema...");
            string analysis = "XSD validation successful";
            Console.WriteLine($"✅ {analysis}");
            return analysis;
        }

        protected override bool ShouldGenerateReport()
        {
            return true;
        }

        protected override void GenerateReport()
        {
            Console.WriteLine($"📊 Generating XML structure report...");
            Console.WriteLine($"   - Format: XML");
            Console.WriteLine($"   - Validation: XSD schema");
            Console.WriteLine($"✅ Report generated");
        }
    }

    public abstract class GameAI
    {
        public void TakeTurn()
        {
            Console.WriteLine($"\n--- {GetAIName()} Turn ---");

            CollectResources();
            BuildStructures();
            BuildUnits();
            SendScouts();

            if (ShouldAttack())
            {
                Attack();
            }
            else
            {
                Defend();
            }

            Console.WriteLine($"--- {GetAIName()} Turn Complete ---");
        }

        protected abstract string GetAIName();
        protected virtual void CollectResources()
        {
            Console.WriteLine($"⛏️ Collecting resources...");
        }

        protected virtual void BuildStructures()
        {
            Console.WriteLine($"🏗️ Building structures...");
        }

        protected virtual void BuildUnits()
        {
            Console.WriteLine($"👷‍♂️ Building units...");
        }

        protected virtual void SendScouts()
        {
            Console.WriteLine($"🔍 Sending scouts...");
        }

        protected abstract bool ShouldAttack();
        protected abstract void Attack();
        protected abstract void Defend();
    }

    public class AggressiveAI : GameAI
    {
        private int _turnCount = 0;

        protected override string GetAIName()
        {
            return "Aggressive AI";
        }

        protected override void BuildUnits()
        {
            Console.WriteLine($"👷 Building OFFENSIVE units (tanks, soldiers)");
            _turnCount++;
        }

        protected override bool ShouldAttack()
        {
            return _turnCount >= 2;
        }

        protected override void Attack()
        {
            Console.WriteLine($"⚔️ ATTACKING in full force");
            Console.WriteLine($"   - Sending all units to enemy base");
            Console.WriteLine($"   - Focus: Destruction");
        }

        protected override void Defend()
        {
            Console.WriteLine($"🛡️ Minimal defense, focusing on offense");
        }
    }

    public class DefensiveAI : GameAI
    {
        protected override string GetAIName()
        {
            return "Defensive AI";
        }

        protected override void BuildStructures()
        {
            Console.WriteLine($"🏗️ BUilding DEFENSIVE structures (walls, towers)");
        }

        protected override void BuildUnits()
        {
            Console.WriteLine($"👷 Building DEFENSIVE units (archers, guards)");
        }

        protected override bool ShouldAttack()
        {
            return new Random().Next(100) < 10;
        }

        protected override void Attack()
        {
            Console.WriteLine($"⚔️ Cautions counter-attack");
            Console.WriteLine($"   - Sending small strike team");
            Console.WriteLine($"   - Focus: Disruption only");
        }

        protected override void Defend()
        {
            Console.WriteLine($"🛡️ FORTIFYING defenses!");
            Console.WriteLine($"   - Strengthening walls");
            Console.WriteLine($"   - Positioning units strategically");
        }
    }

    public class BalancedAI : GameAI
    {
        private int _resources = 100;
        protected override string GetAIName()
        {
            return "Balanced AI";
        }

        protected override void CollectResources()
        {
            base.CollectResources();
            _resources+=50;
            Console.WriteLine($"    Current resources: {_resources}");
        }

        protected override void BuildStructures()
        {
            Console.WriteLine($"🏗️  Buliding BALANCED structures (mix of offense/defense)");
        }

        protected override void BuildUnits()
        {
            Console.WriteLine($"👷  Buidling BALANCED units (versitile army)");
        }

        protected override bool ShouldAttack()
        {
            return _resources >= 200;
        }

        protected override void Attack()
        {
            Console.WriteLine($"⚔️  Strategic attack");
            Console.WriteLine($"   - Balanced army composition");
            Console.WriteLine($"   - Focus Tactical advantage");
            _resources -= 100;
        }

        protected override void Defend()
        {
            Console.WriteLine($"🛡️  Maintaining balanced defense");
        }
    }

    public class TemplatePatternDemo
    {
        public static void Run()
        {
            Console.WriteLine($"=== Template Method Pattern Demo ===\n");

            Console.WriteLine($"--- Data Processing Pipeline ---");

            List<DataProcessor> dataProcessors = new ()
            {
                new CSVDataProcessor(),
                new JSONDataProcessor(),
                new XMLDataProcessor()
            };

            foreach (var processor in dataProcessors)
            {
                processor.ProcessData();
                Console.WriteLine($"{new string('-', 60)}");
            }

            Console.WriteLine($"\n\n--- Game AI Simulation ---");

            List<GameAI> aiPlayers = new()
            {
                new AggressiveAI(),
                new DefensiveAI(),
                new BalancedAI()
            };

            for (int turn = 1; turn <= 3; turn++)
            {
                Console.WriteLine($"\n╔══════════════ TURN {turn} ══════════════╗");
                
                foreach (var ai in aiPlayers)
                {
                    ai.TakeTurn();
                }
                
                Console.WriteLine($"╚════════════════════════════════════╝");
            }
        }
    }
}