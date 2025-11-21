# Dockerfile for UI
FROM node:20-alpine AS builder

WORKDIR /app

# Only copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the app
COPY . .

# Expose Angular dev port
EXPOSE 4200

# Default command (overridden by docker-compose)
CMD ["ng", "serve", "--host", "0.0.0.0"]
