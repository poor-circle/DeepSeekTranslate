﻿using DeepSeekTranslate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using XUnity.AutoTranslator.Plugin.Core.Endpoints;

namespace DeepSeekTranslate
{
    public partial class DeepSeekTranslateEndpoint : ITranslateEndpoint
    {
        private string GetDictStr(IEnumerable<KeyValuePair<string, List<string>>> kvPairs)
        {
            var dictList = new List<string>();
            foreach (var entry in kvPairs)
            {
                var src = entry.Key;
                var dst = entry.Value[0];
                var info = entry.Value[1];
                dictList.Add($"|\t{src}\t|\t{dst}\t|\t{(string.IsNullOrEmpty(info) ? " " : info)}\t|");
            }
            var dictStr = s_dictBaseStr + string.Join("\n", dictList.ToArray()) + "\n--------------------------------------------------";
            return dictStr;
        }

        private string GetSysPromptStr(string originalText)
        {
            string sysPromptStr;
            if (_dictMode == DictMode.None)
            {
                sysPromptStr = _sysPromptStr;
            }
            else if (_dictMode == DictMode.Full)
            {
                sysPromptStr = _sysPromptStr + _fullDictStr;
            }
            else if (_dictMode == DictMode.MatchOriginalText)
            {
                var usedDict = _dict.Where(x => originalText.Contains(x.Key));
                sysPromptStr = _sysPromptStr + (usedDict.Any() ? GetDictStr(usedDict) : string.Empty);
            }
            else
            {
                throw new Exception("Invalid dict mode.");
            }
            return sysPromptStr;
        }
    }
}
