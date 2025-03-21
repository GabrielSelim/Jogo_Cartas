# Use uma imagem base do Node.js
FROM node:20 AS build

# Defina o diretório de trabalho
WORKDIR /app

# Copie os arquivos de configuração do projeto
COPY package*.json ./

# Instale as dependências do projeto
RUN npm install

# Copie o restante dos arquivos do projeto
COPY . .

# Build do projeto Angular
RUN npm run build -- --output-path=dist

# Use uma imagem base do Nginx para servir o front-end
FROM nginx:alpine

# Copie os arquivos buildados para o diretório padrão do Nginx
COPY --from=build /app/dist /usr/share/nginx/html

# Exponha a porta 80
EXPOSE 80

# Comando para iniciar o Nginx
CMD ["nginx", "-g", "daemon off;"]

