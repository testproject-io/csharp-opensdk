// <copyright file="CustomJsonSerializer.cs" company="TestProject">
// Copyright 2020 TestProject (https://testproject.io)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace TestProject.OpenSDK.Internal.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Custom (de-)serialization settings for transforming message objects to and from JSON.
    /// </summary>
    public static class CustomJsonSerializer
    {
        /// <summary>
        /// Returns the default <see cref="JsonSerializerSettings"/>.
        /// </summary>
        public static JsonSerializerSettings DefaultSettings { get; set; } = new JsonSerializerSettings().Populate();

        /// <summary>
        /// Populates the given <see cref="JsonSerializerSettings"/> <paramref name="settings"/>.
        /// </summary>
        /// <param name="settings">Existing JSON serializer settings to customize.</param>
        /// <returns>Customized JSON serializer settings.</returns>
        public static JsonSerializerSettings Populate(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter(new SpaceLowerCaseNamingStrategy(), true));
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        }

        /// <summary>
        /// Serializes the given <paramref name="source"/> object to a JSON string using the given <paramref name="settings"/>, or using the default settings if no <paramref name="settings"/> were provided.
        /// </summary>
        /// <param name="source">The object to be serialized to JSON.</param>
        /// <param name="settings">The JSON serializer settings to use when serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson(this object source, JsonSerializerSettings settings = null)
        {
            if (source == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(source, settings ?? DefaultSettings);
        }

        /// <summary>
        /// Deserializes the <paramref name="source"/> object to a .NET object using the given <paramref name="settings"/>, or using the default settings if no <paramref name="settings"/> were provided.
        /// </summary>
        /// <param name="source">The JSON string to deserialize.</param>
        /// <param name="settings">The JSON serializer settings to use when deserializing.</param>
        /// <returns>The deserialized object.</returns>
        public static object FromJson(this string source, JsonSerializerSettings settings = null)
        {
            if (source == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(source, settings ?? DefaultSettings);
        }

        /// <summary>
        /// Deserializes the <paramref name="source"/> object to a .NET object using the given<paramref name="settings"/>, or using the default settings if no<paramref name="settings"/> were provided.
        /// </summary>
        /// <typeparam name="T">The object type to deserialize the JSON string to.</typeparam>
        /// <param name="source">The JSON string to deserialize.</param>
        /// <param name="settings">The JSON serializer settings to use when deserializing.</param>
        /// <returns>The deserialized object.</returns>
        public static T FromJson<T>(this string source, JsonSerializerSettings settings = null)
        {
            if (source == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(source, settings ?? DefaultSettings);
        }
    }
}
