# BackEnd-Project

Group members :

Boulben Guillaume 23723

Valentin Poli 23750

Lucas Leplanois 23733

Evan Pernette 23721

==================================================================================================================================

Attributions :

Boulben Guillaume : User & Jobs 

Valentin Poli & Evan Pernette : All sub-folders that are linked to the others and debugging

Lucas Leplanois : Employers & Seekers


==================================================================================================================================

Appssetings.json :

{
  "Secret": "JESUISLEROIDUMONDE",
  "ConnectionStrings": {
    "DefaultConnection": "Server=eu-cdbr-west-03.cleardb.net; Database=heroku_e87df673985abe6; Uid=ba4a5f2378a367; Pwd=64aa75a0; Convert Zero Datetime=True"},
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Sendgrid":"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"
  
}

==================================================================================================================================
the link to deployed API :

Dotnet watch run. (SWAGGER)


(DOCKER FILE)
-----------LOGIN_USING_HEROKU_CLI-------------
heroku login
heroku container:login
----------------------------------------------
------PUBLISHING_TO_DOCKER_HEROKU_CLI---------
docker build -t dorset-apia .
docker tag dorset-apia registry.heroku.com/dorset-apia/web
docker push registry.heroku.com/dorset-apia/web
heroku container:push web -a dorset-apia   
heroku container:release web -a dorset-apia
----------------------------------------------