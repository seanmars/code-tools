using ReactiveUI;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ModuleTool.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string srcCode;
        private string resultCode;

        public string SrcCode
        {
            get => srcCode;
            set => this.RaiseAndSetIfChanged(ref srcCode, value);
        }

        public string ResultCode
        {
            get => resultCode;
            set => this.RaiseAndSetIfChanged(ref resultCode, value);
        }

        const string ClassPattern =
            @"(((internal)|(public)|(private)|(protected)|(sealed)|(abstract)|(static))?[\s\r\n\t]+){0,2}class[\s\S]+?(\w+)";
        const string PropertyPattern =
            @"^\s*(?:(?:private|public|protected|internal)\s+)?(?:static\s+)?(?:readonly\s+)?(\w+)\s+(\w+)\s*[^(]";

        public void OnOkClicked()
        {
            if (string.IsNullOrWhiteSpace(SrcCode))
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;")
                .AppendLine();

            var lines = SrcCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var hasClass = false;
            foreach (var l in lines)
            {
                var classMatches = Regex.Matches(l, ClassPattern, RegexOptions.Singleline);
                var propMatches = Regex.Matches(l, PropertyPattern, RegexOptions.Singleline);
                try
                {
                    var classNames = classMatches.Cast<Match>()
                            .Select(x => x.Value.Trim());
                    if (classNames.Any())
                    {
                        var className = classNames
                            .First()
                            .Split(' ')
                            .Last();
                        sb.AppendLine($"[Table(\"{className.ToSnakeCase()}\")]");
                        hasClass = true;
                        continue;
                    }

                    if (!hasClass) { continue; }

                    var propNames = propMatches.Cast<Match>()
                        .Select(x => x.Groups[2].Value.Trim());
                    if (propNames.Any())
                    {
                        var propName = propNames
                            .First()
                            .Split(' ')
                            .Last();
                        sb.AppendLine($"[Column(\"{propName.ToSnakeCase()}\")]");
                        continue;
                    }
                }
                finally
                {
                    sb.AppendLine(l);
                }
            }

            ResultCode = sb.ToString();
        }
    }
}
