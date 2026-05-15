/* global React */
const { useState, useMemo } = React;

// ============ ICONS ============
const Icon = {
  Search: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="18" height="18" {...p}><circle cx="11" cy="11" r="7"/><path d="m20 20-3.5-3.5"/></svg>,
  Cart: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="20" height="20" {...p}><path d="M4 5h2.5l2 12h10l2-8H7"/><circle cx="9" cy="20" r="1.5"/><circle cx="18" cy="20" r="1.5"/></svg>,
  User: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="20" height="20" {...p}><circle cx="12" cy="8" r="4"/><path d="M4 21c0-4.4 3.6-8 8-8s8 3.6 8 8"/></svg>,
  Heart: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="18" height="18" {...p}><path d="M12 21s-7-4.5-9.5-9C1.2 9.4 2.6 6 6 6c2 0 3.5 1.2 4 2.5C10.5 7.2 12 6 14 6c3.4 0 4.8 3.4 3.5 6-2.5 4.5-9.5 9-9.5 9z"/></svg>,
  HeartFill: (p) => <svg viewBox="0 0 24 24" fill="currentColor" width="18" height="18" {...p}><path d="M12 21s-7-4.5-9.5-9C1.2 9.4 2.6 6 6 6c2 0 3.5 1.2 4 2.5C10.5 7.2 12 6 14 6c3.4 0 4.8 3.4 3.5 6-2.5 4.5-9.5 9-9.5 9z"/></svg>,
  Star: (p) => <svg viewBox="0 0 24 24" fill="currentColor" width="14" height="14" {...p}><path d="m12 2 3 7 7.5.5-5.5 4.8 1.8 7.2L12 17.8 5.2 21.5 7 14.3 1.5 9.5 9 9z"/></svg>,
  Plus: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M12 5v14M5 12h14"/></svg>,
  Minus: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M5 12h14"/></svg>,
  Close: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="18" height="18" {...p}><path d="m6 6 12 12M18 6 6 18"/></svg>,
  Check: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" width="18" height="18" {...p}><path d="m5 12 5 5 9-11"/></svg>,
  Arrow: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="18" height="18" {...p}><path d="M5 12h14m-5-6 6 6-6 6"/></svg>,
  ArrowLeft: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="18" height="18" {...p}><path d="M19 12H5m6-6-6 6 6 6"/></svg>,
  Chevron: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="m6 9 6 6 6-6"/></svg>,
  Truck: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="20" height="20" {...p}><path d="M3 7h11v10H3zM14 10h4l3 3v4h-7"/><circle cx="7" cy="18" r="1.7"/><circle cx="17" cy="18" r="1.7"/></svg>,
  Shield: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="20" height="20" {...p}><path d="M12 3 4 6v6c0 5 3.5 8.5 8 9 4.5-.5 8-4 8-9V6z"/><path d="m9 12 2 2 4-4"/></svg>,
  Return: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="20" height="20" {...p}><path d="M3 12a9 9 0 1 0 3-6.7"/><path d="M3 4v5h5"/></svg>,
  Filter: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M4 6h16M7 12h10M10 18h4"/></svg>,
  Trash: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M4 7h16M9 7V4h6v3M6 7l1 13h10l1-13"/></svg>,
  Eye: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12z"/><circle cx="12" cy="12" r="3"/></svg>,
  Edit: (p) => <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="16" height="16" {...p}><path d="M4 20h4l11-11-4-4L4 16zM14 6l4 4"/></svg>,
};

// ============ HEADER ============
function Header({ active = "home", cartCount = 2 }) {
  return (
    <header className="eshop-header">
      <div className="eshop-header__announce">
        FREE SHIPPING ON ORDERS OVER $80 — ENDS SUNDAY
      </div>
      <div className="container">
        <div className="eshop-header__bar">
          <a href="#" className="eshop-header__logo">Maison.</a>
          <nav className="eshop-header__nav">
            <a href="#" className={active === "home" ? "is-active" : ""}>Home</a>
            <a href="#" className={active === "shop" ? "is-active" : ""}>Shop</a>
            <a href="#" className={active === "categories" ? "is-active" : ""}>Categories</a>
            <a href="#">Journal</a>
            <a href="#">About</a>
          </nav>
          <div className="eshop-header__actions">
            <button className="eshop-header__icon-btn" title="Search"><Icon.Search /></button>
            <button className="eshop-header__icon-btn" title="Account"><Icon.User /></button>
            <button className="eshop-header__icon-btn" title="Wishlist">
              <Icon.Heart />
            </button>
            <button className="eshop-header__icon-btn" title="Cart">
              <Icon.Cart />
              {cartCount > 0 && <span className="eshop-header__badge">{cartCount}</span>}
            </button>
          </div>
        </div>
      </div>
    </header>
  );
}

// ============ FOOTER ============
function Footer() {
  return (
    <footer className="eshop-footer">
      <div className="container">
        <div className="eshop-footer__grid">
          <div>
            <div className="eshop-footer__title">Maison.</div>
            <p style={{ maxWidth: 320, color: "var(--ink-2)", marginTop: 12 }}>
              A curated shop for considered objects. Independent makers, careful materials, slow design.
            </p>
            <div style={{ marginTop: 20, display: "flex", gap: 8 }}>
              <input className="field" placeholder="Email for new arrivals" style={{ maxWidth: 240 }} />
              <button className="btn btn--primary btn--sm">Subscribe</button>
            </div>
          </div>
          <div className="eshop-footer__col">
            <h4>Shop</h4>
            <ul>
              <li><a href="#">New arrivals</a></li>
              <li><a href="#">Best sellers</a></li>
              <li><a href="#">All products</a></li>
              <li><a href="#">Gift cards</a></li>
            </ul>
          </div>
          <div className="eshop-footer__col">
            <h4>Care</h4>
            <ul>
              <li><a href="#">Contact</a></li>
              <li><a href="#">Shipping</a></li>
              <li><a href="#">Returns</a></li>
              <li><a href="#">FAQs</a></li>
            </ul>
          </div>
          <div className="eshop-footer__col">
            <h4>Studio</h4>
            <ul>
              <li><a href="#">Our story</a></li>
              <li><a href="#">Journal</a></li>
              <li><a href="#">Trade</a></li>
              <li><a href="#">Stockists</a></li>
            </ul>
          </div>
        </div>
        <div className="eshop-footer__base">
          <span>© 2026 Maison Goods Co.</span>
          <span>Crafted with care</span>
        </div>
      </div>
    </footer>
  );
}

// ============ PRODUCT CARD ============
function Stars({ value = 4, count }) {
  return (
    <span style={{ display: "inline-flex", alignItems: "center", gap: 6 }}>
      <span className="stars">
        {[1,2,3,4,5].map(i => <Icon.Star key={i} style={{ opacity: i <= value ? 1 : 0.2 }} />)}
      </span>
      {count != null && <span style={{ fontSize: 12, color: "var(--muted)" }}>({count})</span>}
    </span>
  );
}

function ProductCard({ product, tag }) {
  return (
    <a className="product-card" href="#">
      <div className="product-card__media">
        <img src={product.img} alt={product.name} />
        {tag && <span className={`product-card__tag ${tag === "Sale" ? "product-card__tag--accent" : tag === "Low stock" ? "product-card__tag--low" : ""}`}>{tag}</span>}
        <button className="product-card__fav" onClick={(e)=>{e.preventDefault();}}><Icon.Heart /></button>
        <button className="product-card__quick" onClick={(e)=>{e.preventDefault();}}>Quick add</button>
      </div>
      <div>
        <div className="product-card__info">
          <div>
            <div className="product-card__name">{product.name}</div>
            <div className="product-card__cat">{product.cat}</div>
          </div>
          <div className="product-card__price">${product.price}</div>
        </div>
        {product.rating && (
          <div style={{ marginTop: 6 }}>
            <Stars value={product.rating} count={product.reviews} />
          </div>
        )}
      </div>
    </a>
  );
}

// ============ DATA ============
const PRODUCTS = [
  { id: 1, name: "Linen Throw Blanket", cat: "Home textiles", price: 128, rating: 5, reviews: 84, img: "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=800&auto=format&fit=crop&q=70", tag: "New" },
  { id: 2, name: "Stoneware Mug, Set of 2", cat: "Kitchen", price: 42, rating: 4, reviews: 156, img: "https://images.unsplash.com/photo-1514228742587-6b1558fcca3d?w=800&auto=format&fit=crop&q=70" },
  { id: 3, name: "Brass Candlestick", cat: "Lighting", price: 86, rating: 5, reviews: 41, img: "https://images.unsplash.com/photo-1602874801007-aa2c1c2bd9bb?w=800&auto=format&fit=crop&q=70", tag: "Low stock" },
  { id: 4, name: "Walnut Cutting Board", cat: "Kitchen", price: 64, rating: 4, reviews: 92, img: "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=800&auto=format&fit=crop&q=70" },
  { id: 5, name: "Ceramic Vase, Tall", cat: "Décor", price: 98, rating: 5, reviews: 38, img: "https://images.unsplash.com/photo-1578500494198-246f612d3b3d?w=800&auto=format&fit=crop&q=70" },
  { id: 6, name: "Hand-woven Basket", cat: "Storage", price: 74, rating: 4, reviews: 67, img: "https://images.unsplash.com/photo-1605883705077-8d3d3cebe78c?w=800&auto=format&fit=crop&q=70", tag: "Sale" },
  { id: 7, name: "Cotton Bath Towel", cat: "Bath", price: 38, rating: 4, reviews: 213, img: "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800&auto=format&fit=crop&q=70" },
  { id: 8, name: "Beeswax Taper Candles", cat: "Lighting", price: 24, rating: 5, reviews: 178, img: "https://images.unsplash.com/photo-1602874801007-aa2c1c2bd9bb?w=800&auto=format&fit=crop&q=70" },
  { id: 9, name: "Oak Serving Bowl", cat: "Kitchen", price: 112, rating: 5, reviews: 29, img: "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=800&auto=format&fit=crop&q=70" },
];

const CATEGORIES = [
  { id: 1, name: "Kitchen", count: 42, img: "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=600&auto=format&fit=crop&q=70" },
  { id: 2, name: "Home textiles", count: 28, img: "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=600&auto=format&fit=crop&q=70" },
  { id: 3, name: "Lighting", count: 19, img: "https://images.unsplash.com/photo-1513506003901-1e6a229e2d15?w=600&auto=format&fit=crop&q=70" },
  { id: 4, name: "Décor", count: 36, img: "https://images.unsplash.com/photo-1578500494198-246f612d3b3d?w=600&auto=format&fit=crop&q=70" },
  { id: 5, name: "Bath", count: 22, img: "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=600&auto=format&fit=crop&q=70" },
  { id: 6, name: "Storage", count: 15, img: "https://images.unsplash.com/photo-1605883705077-8d3d3cebe78c?w=600&auto=format&fit=crop&q=70" },
];

Object.assign(window, {
  Icon, Header, Footer, ProductCard, Stars, PRODUCTS, CATEGORIES,
});
