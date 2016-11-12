﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Phoenix.Extensions;

namespace Phoenix.Models.Vaio
{
    internal class Z13B : Product
    {
        public override string Name => "VAIO Z フリップモデル";
        public override string ModelNumber => "VJZ13B*";
        public override string FeedUrl => "https://support.vaio.com/products/z-vjz131/update.html";

        public override async Task Parse()
        {
            var html = await Get();
            var documentNodes = html.DocumentNode
                                    .SelectSingleNode("//div[@id='tab1']//dl[@class='information information-support']");

            foreach (var nodePair in documentNodes.ChildNodes.Where(w => (w.Name == "dt") || (w.Name == "dd")).Chunk(2))
            {
                var nodes = nodePair.ToList();
                var info = nodes[1];
                var program = new Program
                {
                    Date = DateTime.Parse(nodes[0].InnerText),
                    Name = info.ChildNodes.First(w => w.Name == "a").InnerText,
                    Url = info.ChildNodes.First(w => w.Name == "a").Attributes["href"].Value,
                    Description = info.ChildNodes.Last().InnerText.Trim()
                };
                Softwares.Add(program);
            }
        }
    }
}