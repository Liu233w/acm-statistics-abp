module.exports = {
  extends: ['@commitlint/config-conventional'],
  rules: {
    "subject-case": [0, "never"],
    "scope-case": [0, "never"],
    "header-max-length": [1, "always", 72],
  },
}
