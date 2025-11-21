# Use Node LTS image
FROM node:20-alpine

# Set working directory inside container
WORKDIR /app

# Copy package.json and package-lock.json first (leverages Docker cache)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the app
COPY . ./

# Expose Angular dev server port
EXPOSE 4200

# Start Angular dev server accessible externally
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0"]
