FROM nginx:stable-alpine
COPY ./Resources/Proxy/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]