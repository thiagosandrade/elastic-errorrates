###BUILD ANGULAR SOLUTION###

FROM node:alpine as builder
WORKDIR /usr/src/app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build -- --prod --output-path=dist

###BUILD NGINX AND COPYING FILES###

FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf

COPY --from=builder /usr/src/app/dist /usr/share/nginx/html

WORKDIR /usr/share/nginx/html

EXPOSE 4200