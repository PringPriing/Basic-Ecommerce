/* global React, Header, Footer, ProductCard, PRODUCTS, Icon, Stars */
const { useState: useStateHome } = React;

function PageHome() {
  return (
    <div className="eshop">
      <Header active="home" cartCount={2} />

      {/* Hero */}
      <section className="container" style={{ padding: "64px 32px 80px" }}>
        <div style={{ display: "grid", gridTemplateColumns: "1.1fr 1fr", gap: 56, alignItems: "center" }}>
          <div>
            <div className="eyebrow" style={{ marginBottom: 20 }}>Spring 2026 · Volume 04</div>
            <h1 style={{ fontSize: 84, marginBottom: 24 }}>
              Objects made <em style={{ fontStyle: "italic", color: "var(--accent)" }}>slowly</em>,<br/>
              chosen carefully.
            </h1>
            <p style={{ fontSize: 17, color: "var(--ink-2)", maxWidth: 480, marginBottom: 32, lineHeight: 1.6 }}>
              A curated shop for the home — built around independent makers, honest materials, and pieces that
              earn their place over years, not seasons.
            </p>
            <div style={{ display: "flex", gap: 12 }}>
              <a href="#" className="btn btn--primary btn--lg">Shop the collection <Icon.Arrow /></a>
              <a href="#" className="btn btn--ghost btn--lg">Read the journal</a>
            </div>

            <div style={{ marginTop: 48, display: "flex", gap: 32, paddingTop: 24, borderTop: "1px solid var(--line)" }}>
              <Trust icon={<Icon.Truck />} label="Free shipping over $80" />
              <Trust icon={<Icon.Return />} label="30-day returns" />
              <Trust icon={<Icon.Shield />} label="Secure checkout" />
            </div>
          </div>

          <div style={{ position: "relative", aspectRatio: "4 / 5", borderRadius: 18, overflow: "hidden", background: "var(--surface-2)" }}>
            <img src="https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=900&auto=format&fit=crop&q=75"
              style={{ width: "100%", height: "100%", objectFit: "cover" }} alt="Hero" />
            <div style={{
              position: "absolute", bottom: 24, left: 24, right: 24,
              background: "rgba(250,248,244,0.96)", borderRadius: 12, padding: 16,
              backdropFilter: "blur(8px)", display: "flex", alignItems: "center", justifyContent: "space-between"
            }}>
              <div>
                <div style={{ fontSize: 11, fontFamily: "var(--font-mono)", color: "var(--muted)", letterSpacing: "0.1em", textTransform: "uppercase" }}>Featured</div>
                <div style={{ fontFamily: "var(--font-display)", fontSize: 18, marginTop: 2 }}>Linen Throw Blanket</div>
              </div>
              <span className="price" style={{ fontSize: 22 }}>$128</span>
            </div>
          </div>
        </div>
      </section>

      {/* Featured grid */}
      <section className="container">
        <header style={{ display: "flex", justifyContent: "space-between", alignItems: "end", marginBottom: 32 }}>
          <div>
            <div className="eyebrow">New arrivals</div>
            <h2 style={{ fontSize: 44, marginTop: 8 }}>This season's additions</h2>
          </div>
          <a href="#" className="btn btn--link">View all 142 products <Icon.Arrow /></a>
        </header>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gap: 28 }}>
          {PRODUCTS.slice(0, 6).map(p => <ProductCard key={p.id} product={p} tag={p.tag} />)}
        </div>
      </section>

      {/* Editorial split */}
      <section className="container" style={{ marginTop: 96 }}>
        <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 32, alignItems: "stretch" }}>
          <div style={{ aspectRatio: "1 / 1", borderRadius: 18, overflow: "hidden", background: "var(--surface-2)" }}>
            <img src="https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=900&auto=format&fit=crop&q=75"
              style={{ width: "100%", height: "100%", objectFit: "cover" }} alt="Editorial" />
          </div>
          <div style={{ padding: "32px 16px", display: "flex", flexDirection: "column", justifyContent: "center" }}>
            <div className="eyebrow">From the journal</div>
            <h2 style={{ fontSize: 48, marginTop: 12, marginBottom: 20 }}>
              The case for <em style={{ fontStyle: "italic" }}>fewer, better</em> things.
            </h2>
            <p style={{ fontSize: 16, color: "var(--ink-2)", lineHeight: 1.7, marginBottom: 24 }}>
              We talk to four makers about the materials they refuse to compromise on, and why a kitchen needs
              less than you think. A new essay every month.
            </p>
            <a href="#" className="btn btn--ghost" style={{ alignSelf: "start" }}>Read the essay <Icon.Arrow /></a>
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
}

function Trust({ icon, label }) {
  return (
    <div style={{ display: "flex", alignItems: "center", gap: 10, fontSize: 13, color: "var(--ink-2)" }}>
      <span style={{ color: "var(--accent)" }}>{icon}</span>{label}
    </div>
  );
}

window.PageHome = PageHome;
