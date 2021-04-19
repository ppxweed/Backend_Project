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