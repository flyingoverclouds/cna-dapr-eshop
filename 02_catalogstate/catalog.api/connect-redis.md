docker exec -it dapr_redis redis-cli

keys *

hgetall "myapp||name"
