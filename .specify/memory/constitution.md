<!--
Sync Impact Report
==================
Version: 0.0.0 → 1.0.0
Reason: Initial constitution ratification

Modified Principles:
- N/A (initial creation)

Added Sections:
- I. Modular Monolith Architecture
- II. Test-Required Development
- III. Simplicity & YAGNI
- Development Standards
- Code Quality
- Governance

Removed Sections:
- N/A

Templates Requiring Updates:
- ✅ plan-template.md - Constitution Check section already references constitution file dynamically
- ✅ spec-template.md - No hard-coded principle references found
- ✅ tasks-template.md - Updated test language from "OPTIONAL" to "REQUIRED per Constitution" to align with Test-Required principle
- ✅ All command files - No agent-specific references found (generic guidance only)

Follow-up TODOs:
- None
-->

# Best Blogs Constitution

## Core Principles

### I. Modular Monolith Architecture

Every feature must be organized into clear, cohesive modules within a single codebase. Modules MUST:

- Have well-defined boundaries with explicit interfaces
- Be independently understandable without examining other modules
- Expose functionality through documented public APIs
- Minimize cross-module dependencies (prefer one-way dependencies)
- Keep related code together (high cohesion, low coupling)

**Rationale**: Modular monoliths provide the organizational benefits of microservices (clear boundaries, independent reasoning) without the operational complexity (distributed debugging, network latency, deployment orchestration). This architecture enables teams to move quickly while maintaining code quality and allows future extraction to services if needed.

### II. Test-Required Development

Tests MUST be delivered with every feature. For each user story:

- Contract tests are required for all public APIs and module boundaries
- Integration tests are required for multi-module interactions
- Unit tests are recommended but not mandatory for internal implementation details
- Tests must verify acceptance criteria defined in specifications
- All tests must pass before code review approval

**Rationale**: Requiring tests ensures quality without imposing the workflow overhead of strict TDD. This approach balances velocity with assurance, allowing developers flexibility in when they write tests (before, during, or after implementation) while maintaining non-negotiable quality gates.

### III. Simplicity & YAGNI

Implementations MUST be the simplest solution that satisfies current requirements.

- Do not build for hypothetical future requirements (YAGNI - You Aren't Gonna Need It)
- Prefer three lines of similar code over premature abstraction
- Only add error handling for scenarios that can actually occur
- Trust internal code and framework guarantees
- Validate only at system boundaries (user input, external APIs)
- Avoid helper functions, utilities, or abstractions for one-time operations
- No feature flags or backward-compatibility shims when you can just change the code

**Rationale**: Complexity is the enemy of velocity and maintainability. Simple code is easier to understand, debug, and modify. Adding complexity for hypothetical futures creates maintenance burden and cognitive load without delivering current value. Start simple; refactor when actual needs emerge with concrete requirements.

## Development Standards

### Module Organization

- Group code by feature/domain, not by technical layer
- Each module should represent a cohesive business capability
- Shared utilities and cross-cutting concerns in dedicated modules
- Clear README per module documenting purpose and public API

### Dependency Management

- Document all module dependencies explicitly
- Avoid circular dependencies between modules
- Prefer compile-time dependency detection over runtime discovery
- Keep third-party dependencies minimal and well-justified

### Interface Design

- Public APIs must have clear contracts (input, output, errors)
- Use types/schemas to document API boundaries
- Version breaking changes to module interfaces explicitly
- Internal implementation details should not leak through APIs

## Code Quality

### Documentation Requirements

- Module-level README explaining purpose, public API, and examples
- Public API documentation (function/method signatures with descriptions)
- Inline comments ONLY where logic is non-obvious
- No documentation for trivial getters/setters or self-evident code

### Code Review Standards

- Verify module boundaries are respected (no boundary violations)
- Check tests cover acceptance criteria from specifications
- Ensure simplicity principle applied (no unnecessary abstraction)
- Validate error handling only at appropriate boundaries
- Confirm no over-engineering or YAGNI violations

### Testing Standards

- Contract tests verify module API contracts
- Integration tests verify cross-module interactions
- Tests should be independent and repeatable
- Test names should clearly describe what is being verified
- Avoid test duplication across contract/integration/unit layers

## Governance

### Amendment Process

1. Proposed changes MUST be documented with rationale
2. Impact analysis required for all dependent templates and workflows
3. Version bump according to semantic versioning (see below)
4. Migration plan required for breaking changes
5. All team members must acknowledge updated constitution

### Versioning Policy

- **MAJOR**: Backward incompatible changes (principle removal/redefinition, architecture changes)
- **MINOR**: New principles added or material expansion of guidance
- **PATCH**: Clarifications, wording improvements, typo fixes

### Compliance & Review

- All PRs must be reviewed against constitution principles
- Code reviews MUST verify Test-Required compliance
- Complexity and abstraction MUST be justified against Simplicity principle
- Constitution violations require explicit justification or rejection
- Quarterly constitution review to ensure principles remain relevant

### Runtime Development Guidance

For detailed implementation workflows and command-specific guidance, refer to the command files in `.claude/commands/speckit.*.md`. These files provide executable instructions that align with constitutional principles.

**Version**: 1.0.0 | **Ratified**: 2025-12-06 | **Last Amended**: 2025-12-06
