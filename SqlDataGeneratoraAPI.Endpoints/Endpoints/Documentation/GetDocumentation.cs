using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Security.AccessControl;

namespace SQLDataGeneratorAPI.Endpoints.Endpoints.Documentation
{
    public static class GetDocumentation
    {
        public static OpenApiResponse Return200(string description, string objectName, string sample)
        {
            var response = new OpenApiResponse
            {
                Description = description,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "array",
                            Items = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    [objectName] = new OpenApiSchema { Type = "string", Example = new OpenApiString(sample) }
                                }
                            }
                        },
                        Example = new OpenApiArray
                        {
                            new OpenApiObject
                            {
                                [objectName] = new OpenApiString(sample + " 1")
                            },
                            new OpenApiObject
                            {
                                [objectName] = new OpenApiString(sample + " 2")
                            }
                        }
                    }
                }
            };
            return response;
        }
        public static OpenApiResponse Return400()
        {
            var response = new OpenApiResponse
            {
                Description = "Invalid input parameters.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Invalid parameter given.") }
                            }
                        }
                    }
                }
            };
            return response;

        }
        public static OpenApiResponse Return401()
        {
            var response = new OpenApiResponse
            {
                Description = "Invalid/Doesn't include API key.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Not authorized.") }
                            }
                        }
                    }
                }
            };
            return response;

        }
        public static OpenApiResponse Return500()
        {
            var response = new OpenApiResponse
            {
                Description = "Internal server error.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("There has been an error.") }
                            }
                        }
                    }
                }
            };
            return response;

        }
    }
}
