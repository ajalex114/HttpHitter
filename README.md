# HttpHitter
This is a .net console app, which supports hitting rest endpoints with custom support for headers, variables etc.

The 3 main components of the application are the three files `config.json`, `Headers.prop` and `content.csv`.

These files, if found in the `Asset` folder, will be picked up by the application by default.
Alternatively, to override any of these files, use the following command to execute the app :  

``` bat
HttpHitter.exe /config <configFileName> /header <headerFileName> /content <contentFileName>
```

-------------------------
## How to configure the app

The app configuration is taken from the above said 3 files.

### Config File (config.json)
-----------

This file holds the basic configuration for the app to run
It contains the following parameters : 

#### `"maxRun"`  
------------------

|    |    |
|----|----|
| Data Type | `ulong` |
| Range     | `0` to `18,446,744,073,709,551,615` |

_Description_ : This specifies the maximum number of requests that will be made by the application.

#### `"order"`

|    |    |
|----|----|
| Data Type | `enum` |

Supported Values :  
>    `Sequential`  
>    `SequentialRepeat`  
>    `Random`  
>    `RandomRepeat`  

_Description :_

* `Sequential` :  
The requests - as provided in the `content.csv`- are executed in a _sequential_ order.  
If there are 10 requests in `content.csv` file, and `maxRun = 100`, a total of **10** requests are executed.


* `SequentialRepeat` :  
The requests - as provided in the `content.csv`- are executed in a _sequential_ order, and the requests are repeated.  
If there are 10 requests in `content.csv` file, and `maxRun = 100`, a total of **100** requests are executed.

* `Random` :  
The requests - as provided in the `content.csv`- are executed in a _random_ order.  
If there are 10 requests in `content.csv` file, and `maxRun = 100`, a total of **10** requests are executed.


* `RandomRepeat` :  
The requests - as provided in the `content.csv`- are executed in a _random_ order, and the requests are repeated.  
If there are 10 requests in `content.csv` file, and `maxRun = 100`, a total of **100** requests are executed.

#### `"defaultVariables"`
------------------

This is a `Dictionary<string, string>`, which holds the default variables, which will be applied on requests as provided in `content.csv` file.
The `Key` is the variable name and the `Value` is the variable value.

#### `"defaultMultiVariables"`
----------------

This is a `Dictionary<string, string[]>`, which holds the default variables, which will be applied on requests as provided in `content.csv` file.
The `Key` is the variable name and the `Value` is a collection of values which will be applied to the variable.

#### Example
----------------

``` json
{
  "maxRun": 2000,
  "order": "RandomRepeat",
  "defaultVariables": {
    "host": "http://localhost:8080/"    
  },
  "defaultMultiVariables": {
    "id": [
        "1",
        "2",
        "3"
    ],
  }
}
```

-------------------------------

### Headers File (Headers.prop)
-----------------
This is a property file which holds the `Header Values`.  
It is a `Key-Value` based file, where the Key and Value are separated by `:<space>`.  
_Note that, it is a colon followed by a space_.

#### Example
-----------

```
Authorization: <Authorization Code>
Cookie: <Cookie>
```
--------------------------

### Content File (content.csv)
----------------------

Content file is the file which provides request information to be executed.  
It is basically a comma separated value (csv) file.  
Each csv field is a combination of 2 or 3 values, separated by `::`  

It is of the format :
```
<Key>::<Content Type>[::<Value>]  
```
_Note that, `::<value>` is not a mandatory field._

The various `Content Types` supported are : 
> `url`  
> `query`  
> `var`  
> `multiVar`  

* `url`  
It represents a url. The Key is the url. It does not require the `::<value>` field.  

For example : 
```
http://localhost:8080::url
```

* `query`
It represents a Query Parameter. The Key is the query parameter and the Value is the query parameter value.  

For example : 
```
http://localhost:8080::url,param::query::1
```

will be translated as : `http://localhost:8080?param=1`

* `var`  
This represents a variable.  
Any variable can be provided in the content file. This variable will override any variable provided in `config file` in the `defaultVariable` or `defaultMultiVariable` fields.  

For example : 
```
http://localhost:{port}::url,port::var::8080
```
will be translated as : `http://localhost:8080`

The variable can also be applied on query parameters as well.  
For example : 
```
http://localhost:{port}::url,param::query::{parameter},port::var::8080,parameter::var::1
```
will be translated as : `http://localhost:8080?param=1`

As seen above, multiple variables can be provided in the same content request.

* `multiVar`  
This represents a multi variable. This variable can hold a collection of values, just like `defaultMultiVariables`.  
Unlike in `defaultMultiVariables`, `multiVar` is provided in the same line, as the request has to be constrained in a single line.  
The values are separated by a `;` in a `multiVar`.  

For example : 

```
http://localhost:{port}/values/{id}::url,port::var::8080,id::multiVar::1;2;3;4
```

This will be translated into 4 requests as below :   
`http://localhost:8080/values/1?param=1`  
`http://localhost:8080/values/2?param=1`  
`http://localhost:8080/values/3?param=1`  
`http://localhost:8080/values/4?param=1`  

#### Extras
------------------

The content file supports multiple requests. Each line is considered as a single request.  
A request can have only 1 `url`.  
Any number of `query`, `var` or `multiVar` is supported in a single request.  
Comments are supported. If a line starts with `#`, then the line is ignored.

### Order of Variable replacement

It is possible to give the same name for various types of variables. In this case, the higher priority variable ends up overriding any other variable values.

The priority is as below : 

> `defaultMultiVariables < defaultVariables < multiVar < var`

If a `var` is provided on a request, it will override any other value specified elsewhere.  
If a `multiVar` is provided, any value specified by `defaultVariables` or `defaultMultiVariables` will be overriden.  
If a `defaultVariable` is provided, any value specified by `defaultMultiVariables` will be overriden.  

### Execution model for multi variable support

Every time a multi variable is provided as part of `defaultMultiVariable` or `multiVar` or a combination of both, a permutation-combination of all the possible variable values are applied on every single request.  
This means, a single line request will end up creating `<permutation-combination number>` of requests.

For example : 
```
{scheme}://localhost:8080/values/{id}::url,id::multiVar::1;2,scheme::multiVar::http,https
```
This will translate to 4 requests as below :   
`http://localhost:8080/values/1`  
`http://localhost:8080/values/2`  
`https://localhost:8080/values/1`  
`https://localhost:8080/values/2`  