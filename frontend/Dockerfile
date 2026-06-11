
# build 
FROM node:18-alpine AS build
WORKDIR /app

# Copia os manifestos primeiro para aproveitar o cache.
COPY package*.json ./
RUN npm install

# Copia o restante do codigo e gera o build de producao.
COPY . .

# URL da API consumida pelo navegador (pode ser sobrescrita no build).
ARG REACT_APP_API_URL=http://localhost:5256/api
ENV REACT_APP_API_URL=$REACT_APP_API_URL

RUN npm run build

# imagem final (servidor estatico)
FROM node:18-alpine AS final
WORKDIR /app

# "serve" e um servidor estatico simples para SPA.
RUN npm install -g serve

# Copia apenas os arquivos ja compilados.
COPY --from=build /app/build ./build

# O frontend escuta na porta 3000 dentro do container.
EXPOSE 3000

# -s = modo SPA (todas as rotas caem no index.html)
CMD ["serve", "-s", "build", "-l", "3000"]
