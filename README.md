dotnet ef migrations add Create_Database -p ./src/BookStore.Infrastructure -s ./src/BookStore.Api

http://localhost:18080/admin  admin/admin

docker-compose up --build
docker-compose start
docker-compose stop
docker-compose ps
docker-compose down