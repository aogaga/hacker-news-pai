# Use Node.js LTS
FROM node:20-alpine

WORKDIR /app

# Install Angular CLI globally
RUN npm install -g @angular/cli

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install --legacy-peer-deps

# Copy rest of the app
COPY . .

# Expose Angular dev server port
EXPOSE 4200

# Default command: delete node_modules, install dependencies, serve app
CMD ["/bin/sh", "-c", "rm -rf /app/node_modules && npm install --legacy-peer-deps && ng serve --host 0.0.0.0"]
