
FROM node:lts-alpine as build-stage

WORKDIR /app

COPY ./BudgetBringer ./

RUN rm -rf node_modules package-lock.json && npm install

RUN npm run build

FROM nginx:stable-alpine as production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY default.conf /etc/nginx/conf.d/default.conf 

EXPOSE 88
CMD ["nginx", "-g", "daemon off;"]
