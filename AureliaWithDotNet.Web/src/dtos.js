/* Options:
Date: 2020-06-13 21:10:29
Version: 5.91
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: https://localhost:5001

//GlobalNamespace:
//MakePropertiesOptional: False
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion:
//AddDescriptionAsComments: True
//IncludeTypes:
//ExcludeTypes:
//DefaultImports:
*/
var HelloResponse = /** @class */ (function () {
    function HelloResponse(init) {
        Object.assign(this, init);
    }
    return HelloResponse;
}());
export { HelloResponse };
// @Route("/hello")
// @Route("/hello/{Name}")
var Hello = /** @class */ (function () {
    function Hello(init) {
        Object.assign(this, init);
    }
    Hello.prototype.createResponse = function () { return new HelloResponse(); };
    Hello.prototype.getTypeName = function () { return 'Hello'; };
    return Hello;
}());
export { Hello };
//# sourceMappingURL=dtos.js.map