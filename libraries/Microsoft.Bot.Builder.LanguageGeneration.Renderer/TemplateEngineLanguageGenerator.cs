﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    /// <summary>
    /// ILanguageGenerator implementation which uses TemplateEngine. 
    /// </summary>
    public class TemplateEngineLanguageGenerator : ILanguageGenerator
    {
        private const string DEFAULTLABEL = "Unknown";
        private TemplateEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineLanguageGenerator"/> class.
        /// </summary>
        public TemplateEngineLanguageGenerator()
        {
            this.engine = new TemplateEngine();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineLanguageGenerator"/> class.
        /// </summary>
        /// <param name="lgText">lg template text.</param>
        /// <param name="importResolver">template resource loader (id) => templateText.</param>
        /// <param name="id">optional label for the source of the templates (used for labeling source of template errors).</param>
        public TemplateEngineLanguageGenerator(string lgText, string id = null, ImportResolverDelegate importResolver = null)
        {
            this.Id = id ?? DEFAULTLABEL;
            this.engine = new TemplateEngine().AddText(lgText ?? String.Empty, this.Id, importResolver: importResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineLanguageGenerator"/> class.
        /// </summary>
        /// <param name="engine">template engine.</param>
        /// <param name="name">optional label for the source of the templates (used for labeling source of template errors).</param>
        public TemplateEngineLanguageGenerator(TemplateEngine engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// Gets or sets id of the source of this template (used for labeling errors).
        /// </summary>
        /// <value>
        /// Id of the source of this template (used for labeling errors).
        /// </value>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Method to generate text from given template and data.
        /// </summary>
        /// <param name="turnContext">context.</param>
        /// <param name="template">template to evaluate.</param>
        /// <param name="data">data to bind to.</param>
        /// <returns>generated text.</returns>
        public async Task<string> Generate(ITurnContext turnContext, string template, object data)
        {
            try
            {
                return engine.Evaluate(template, data);
            }
            catch (Exception err)
            {
                if (!String.IsNullOrEmpty(this.Id))
                {
                    throw new Exception($"{Id}:{err.Message}");
                }

                throw;
            }
        }
    }
}
