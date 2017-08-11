# Fun Facts

The project Fun Facts is a REST API to provide fun facts about a topic. It allows the following:

- Retrieve a random fact
- Retrieve the top X most popular fun facts, where x can be a non negative integer (3, 5, 10, etc.).
- Add a fun fact.
- Modify a fun fact.
- Remove a fun fact.

### Tech Discussion
The implemented API is intended for Chuck Norris fun facts. But it is a implementation of a generic fun facts API that can be reused for other types of fun facts. It is not possible to create a single REST API that work with any type of fun fact because the MVC model binder for POST and PUT actions must be specified a concreet type to be able to bind the model in the request.

The REST API has the authorization disabled for POST, PUT and DELETE actions. In production Authentication should be implemented and authorization should be enabled.

By default the API connects to an Azure SQL Database. You may choose a different database by adding a connection string and passing its name to the FunFactsContext class.

No DTOs are used in and out of the API. For such a simple case there is no need to hide information to the clients or to prevent overposting. Decoupling could be an issue in the future.

Unfortunatelly, the API help documentation cannot pick the help summaries of the generic class and reuse them in their implementations. The only way to solve this is to override the generic methods and add the summary there.

The project is using this libraries:
- Ninject for IOC container.
- Moq for mocking in unit test.
- Entity Framework as ORM.

### Building for source
Clone or Download the repository.
Open the FunFacts.sln file in Visual Studio 2017. 

### Run locally
To run locally the REST API press F5 in Visual Studio.
A web browser opens click the *Run Tests* button.
### Using the REST API from Javascript:
##### Get
To get a random fun fact:
```javascript
$.get("/api/ChuckNorrisFunFacts", {},
    function (model) {
        console.log("getRandom " + JSON.stringify(model));
    });
```
To get the top 20 rated fun facts:
```javascript
 $.get("/api/ChuckNorrisFunFacts", {count: 20},
    function (model) {
        console.log("getTop " + JSON.stringify(model));
    });
```

##### Post
To add a new element:
```javascript
 $.post("/api/ChuckNorrisFunFacts", newFact )
    .done(function (model) {
        console.log("post " + JSON.stringify(model));
    });
```
##### Put
To modify an existing element:
```javascript
$.post("/api/ChuckNorrisFunFacts", newFact )
    .done(function (model) {
        newFact = model;
    })
    .done(function (model) {
        newFact.Fact = "Some Fact3 Changed";
        $.ajax({
            url: '/api/ChuckNorrisFunFacts/' + model.Id,
            type: 'PUT',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newFact2),
            dataType: 'json'
        })
            .done(function (model) {
                console.log("put " + JSON.stringify(model));
            })
```
##### Delete
To delete an existing element:
```javascript
$.post("/api/ChuckNorrisFunFacts", newFact )
    .done(function (model) {
        newFact = model;
    })
    .done(function (model) {
        $.ajax({
            url: '/api/ChuckNorrisFunFacts/' + model.Id,
            type: 'DELETE',
            contentType: "application/json; charset=utf-8",
        })
            .done(function (model) {
                console.log("put " + JSON.stringify(model));
            })
```

License
----

MIT

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)


   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [jQuery]: <http://jquery.com>

