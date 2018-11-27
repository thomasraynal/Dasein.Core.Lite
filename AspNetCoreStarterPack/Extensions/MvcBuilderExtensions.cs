using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreStarterPack.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder RegisterJsonSettings(this IMvcBuilder builder, JsonSerializerSettings settings)
        {
            builder.Services.AddSingleton(settings);
            builder.Services.AddSingleton(JsonSerializer.Create(settings));

            return builder.AddJsonOptions(options =>
                       {
                           options.SerializerSettings.Culture = settings.Culture;
                           options.SerializerSettings.CheckAdditionalContent = settings.CheckAdditionalContent;
                           options.SerializerSettings.ConstructorHandling = settings.ConstructorHandling;
                           options.SerializerSettings.Context = settings.Context;
                           options.SerializerSettings.DateFormatHandling = settings.DateFormatHandling;
                           options.SerializerSettings.DateFormatString = settings.DateFormatString;
                           options.SerializerSettings.DateParseHandling = settings.DateParseHandling;
                           options.SerializerSettings.DateTimeZoneHandling = settings.DateTimeZoneHandling;
                           options.SerializerSettings.DefaultValueHandling = settings.DefaultValueHandling;
                           options.SerializerSettings.EqualityComparer = settings.EqualityComparer;
                           options.SerializerSettings.Error = settings.Error;
                           options.SerializerSettings.FloatFormatHandling = settings.FloatFormatHandling;
                           options.SerializerSettings.FloatParseHandling = settings.FloatParseHandling;
                           options.SerializerSettings.Formatting = settings.Formatting;
                           options.SerializerSettings.MaxDepth = settings.MaxDepth;
                           options.SerializerSettings.MetadataPropertyHandling = settings.MetadataPropertyHandling;
                           options.SerializerSettings.MissingMemberHandling = settings.MissingMemberHandling;
                           options.SerializerSettings.NullValueHandling = settings.NullValueHandling;
                           options.SerializerSettings.ObjectCreationHandling = settings.ObjectCreationHandling;
                           options.SerializerSettings.PreserveReferencesHandling = settings.PreserveReferencesHandling;
                           options.SerializerSettings.ReferenceLoopHandling = settings.ReferenceLoopHandling;
                           options.SerializerSettings.ReferenceResolverProvider = settings.ReferenceResolverProvider;
                           options.SerializerSettings.SerializationBinder = settings.SerializationBinder;
                           options.SerializerSettings.StringEscapeHandling = settings.StringEscapeHandling;
                           options.SerializerSettings.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
                           options.SerializerSettings.TypeNameHandling = settings.TypeNameHandling;
                           options.SerializerSettings.ContractResolver = settings.ContractResolver;
                           options.SerializerSettings.Converters = settings.Converters;

                       });
        }
    }
}
