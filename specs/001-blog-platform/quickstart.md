# Quickstart Guide: Lifestyle Blog Platform

**Last Updated**: 2025-12-06
**Feature**: 001-blog-platform

## Prerequisites

Before starting development, ensure you have:

- **Node.js**: 20 LTS or higher
- **pnpm**: 8.x or higher (recommended) or npm 9.x+
- **PostgreSQL**: 16 or higher
- **Git**: Latest version
- **Code Editor**: VS Code recommended (with TypeScript, ESLint, Prettier extensions)

## Project Setup

### 1. Clone and Initialize

```bash
# Clone the repository
git clone <repository-url>
cd best-blogs

# Checkout the feature branch
git checkout 001-blog-platform

# Install dependencies for both frontend and backend
pnpm install
```

### 2. Database Setup

```bash
# Start PostgreSQL (if using Docker)
docker run --name best-blogs-postgres \
  -e POSTGRES_USER=bloguser \
  -e POSTGRES_PASSWORD=blogpass \
  -e POSTGRES_DB=bestblogsdb \
  -p 5432:5432 \
  -d postgres:16

# Or use local PostgreSQL installation
createdb bestblogsdb
```

### 3. Environment Configuration

Create `.env` files for backend and frontend:

#### Backend `.env` (backend/.env)

```env
# Database
DATABASE_HOST=localhost
DATABASE_PORT=5432
DATABASE_USER=bloguser
DATABASE_PASSWORD=blogpass
DATABASE_NAME=bestblogsdb

# Server
PORT=3000
NODE_ENV=development

# OAuth - Google
GOOGLE_CLIENT_ID=your-google-client-id
GOOGLE_CLIENT_SECRET=your-google-client-secret
GOOGLE_CALLBACK_URL=http://localhost:3000/api/admin/auth/google/callback

# OAuth - GitHub
GITHUB_CLIENT_ID=your-github-client-id
GITHUB_CLIENT_SECRET=your-github-client-secret
GITHUB_CALLBACK_URL=http://localhost:3000/api/admin/auth/github/callback

# Session
SESSION_SECRET=your-secret-key-min-32-chars-long
SESSION_MAX_AGE=604800000

# CORS
FRONTEND_URL=http://localhost:5173
```

#### Frontend `.env` (frontend/.env)

```env
VITE_API_URL=http://localhost:3000/api
VITE_APP_NAME=Best Blogs
```

### 4. OAuth Setup (Development)

#### Google OAuth

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing
3. Navigate to "APIs & Services" → "Credentials"
4. Create OAuth 2.0 Client ID
5. Add authorized redirect URI: `http://localhost:3000/api/admin/auth/google/callback`
6. Copy Client ID and Client Secret to backend `.env`

#### GitHub OAuth

1. Go to [GitHub Settings](https://github.com/settings/developers)
2. Click "New OAuth App"
3. Application name: "Best Blogs (Dev)"
4. Homepage URL: `http://localhost:5173`
5. Authorization callback URL: `http://localhost:3000/api/admin/auth/github/callback`
6. Copy Client ID and Client Secret to backend `.env`

### 5. Run Database Migrations

```bash
cd backend

# Run migrations to create database schema
pnpm run migration:run

# Seed development data (optional)
pnpm run seed:dev
```

### 6. Start Development Servers

Open two terminal windows:

#### Terminal 1: Backend

```bash
cd backend
pnpm run dev

# Backend will start on http://localhost:3000
# API endpoints available at http://localhost:3000/api
```

#### Terminal 2: Frontend

```bash
cd frontend
pnpm run dev

# Frontend will start on http://localhost:5173
# SSR development server with HMR enabled
```

## Project Structure Overview

```
best-blogs/
├── backend/                 # Express.js API server
│   ├── src/
│   │   ├── modules/        # Feature modules (blog, comments, admin, categories)
│   │   ├── shared/         # Shared utilities and config
│   │   └── server.ts       # Express app entry point
│   ├── tests/              # Backend tests
│   └── package.json
│
├── frontend/               # React + Vite SSR app
│   ├── src/
│   │   ├── modules/        # Feature modules (blog, comments, admin)
│   │   ├── shared/         # Shared components, styles, API client
│   │   ├── entry-client.tsx # Client entry (hydration)
│   │   ├── entry-server.tsx # Server entry (SSR)
│   │   └── App.tsx
│   ├── tests/              # Frontend tests
│   ├── e2e/                # Playwright E2E tests
│   └── package.json
│
├── shared/                 # Shared TypeScript types
│   └── types/
│
└── specs/                  # Feature specifications and plans
    └── 001-blog-platform/
```

## Development Workflow

### Running Tests

```bash
# Backend tests
cd backend
pnpm test                   # Run all tests
pnpm test:watch            # Watch mode
pnpm test:coverage         # With coverage report

# Frontend tests
cd frontend
pnpm test                   # Component + integration tests
pnpm test:e2e              # Playwright E2E tests
pnpm test:e2e:ui           # E2E with Playwright UI
```

### Code Quality

```bash
# Both frontend and backend support:

# Linting
pnpm lint                   # Check for linting errors
pnpm lint:fix              # Auto-fix linting errors

# Type checking
pnpm typecheck             # TypeScript type checking

# Formatting
pnpm format                # Format code with Prettier
pnpm format:check          # Check formatting without changes
```

### Database Operations

```bash
cd backend

# Create a new migration
pnpm run migration:create -- MigrationName

# Run pending migrations
pnpm run migration:run

# Revert last migration
pnpm run migration:revert

# Drop all tables and re-run migrations (⚠️ destructive)
pnpm run schema:drop && pnpm run migration:run
```

## Accessing the Application

### Public Site

- **Homepage**: http://localhost:5173
- **Blog Post**: http://localhost:5173/posts/{post-id}

### Admin Panel

- **Login**: http://localhost:5173/admin/login
- **Dashboard**: http://localhost:5173/admin (requires authentication)

### API Endpoints

- **Blog API**: http://localhost:3000/api/blog
- **Comments API**: http://localhost:3000/api/comments
- **Admin API**: http://localhost:3000/api/admin (requires auth)

## Common Development Tasks

### Creating a New Blog Post (via API)

```bash
# 1. Authenticate via OAuth (visit in browser)
open http://localhost:5173/admin/login

# 2. Create post via API (with session cookie)
curl -X POST http://localhost:3000/api/admin/posts \
  -H "Content-Type: application/json" \
  -b cookies.txt \
  -d '{
    "title": "My First Post",
    "content": "This is the full content of my blog post...",
    "authorName": "Admin",
    "categoryId": "00000000-0000-0000-0000-000000000001"
  }'
```

### Submitting a Comment

```bash
curl -X POST http://localhost:3000/api/comments/posts/{post-id}/comments \
  -H "Content-Type: application/json" \
  -d '{
    "commenterName": "John Doe",
    "commenterEmail": "john@example.com",
    "content": "Great post! Really enjoyed reading this."
  }'
```

### Viewing Logs

```bash
# Backend logs (structured JSON in production, pretty in development)
cd backend
pnpm run dev | pnpm dlv  # Pretty logging with pino-pretty

# Frontend Vite dev server logs
cd frontend
pnpm run dev
```

## Troubleshooting

### Database Connection Issues

```bash
# Check PostgreSQL is running
psql -U bloguser -d bestblogsdb -c "SELECT 1"

# Verify connection settings in backend/.env
cat backend/.env | grep DATABASE

# Reset database (⚠️ destroys all data)
dropdb bestblogsdb && createdb bestblogsdb
cd backend && pnpm run migration:run
```

### OAuth Authentication Issues

```bash
# Verify OAuth credentials are set
cat backend/.env | grep GOOGLE
cat backend/.env | grep GITHUB

# Check callback URLs match exactly
# Google: http://localhost:3000/api/admin/auth/google/callback
# GitHub: http://localhost:3000/api/admin/auth/github/callback
```

### Port Conflicts

```bash
# If port 3000 or 5173 is already in use:

# Find and kill process on port 3000
lsof -ti:3000 | xargs kill -9

# Find and kill process on port 5173
lsof -ti:5173 | xargs kill -9

# Or change ports in .env files
```

### SSR Hydration Errors

```bash
# Clear browser cache and reload
# Check browser console for specific hydration mismatch errors
# Ensure server-rendered HTML matches client-side React output
```

## Next Steps

1. Review the [Implementation Plan](./plan.md)
2. Check the [Data Model](./data-model.md) for database schema details
3. Review [API Contracts](./contracts/) for endpoint specifications
4. Run `/speckit.tasks` to generate detailed task breakdown
5. Start implementing User Story 1 (Browse and Read Blog Posts)

## Useful Resources

- **React Documentation**: https://react.dev
- **Vite SSR Guide**: https://vitejs.dev/guide/ssr
- **TypeORM Documentation**: https://typeorm.io
- **React Router**: https://reactrouter.com
- **Passport.js**: http://www.passportjs.org
- **SCSS Documentation**: https://sass-lang.com/documentation
- **Vitest**: https://vitest.dev
- **Playwright**: https://playwright.dev

## Getting Help

If you encounter issues:

1. Check this quickstart guide for troubleshooting steps
2. Review the [spec.md](./spec.md) for requirements clarification
3. Consult the [research.md](./research.md) for technology decisions
4. Check the project constitution in `.specify/memory/constitution.md`
