# ==============================================================================
# BillingRadar Frontend - Vue.js & Vite Dockerfile
# ==============================================================================

# Stage 1: Build Vue application
FROM node:26-alpine AS build-stage
WORKDIR /app

# Copy package configurations and install dependencies
COPY package*.json ./
RUN npm install

# Copy source code and build production bundle
COPY . .
RUN npm run build

# Stage 2: Serve application via Nginx
FROM nginx:stable-alpine AS production-stage

# Copy built static files to Nginx default html path
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Copy a basic custom nginx configuration to handle SPA routing (optional but recommended)
RUN echo 'server { \
    listen 80; \
    location / { \
        root /usr/share/nginx/html; \
        index index.html index.htm; \
        try_files $uri $uri/ /index.html; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
